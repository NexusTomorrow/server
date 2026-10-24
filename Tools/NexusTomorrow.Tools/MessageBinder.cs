using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Tools;

/// <summary>
/// Sets a message's wire members from JSON, every member given and nothing else, so an injected message carries exactly the values its
/// experiment names. The members are the generator's: a wire base type's first, then instance properties (or fields) declared in source
/// without <see cref="NotOnWireAttribute"/>, in declaration order, as the protocol reference's layout walk reads them.
/// </summary>
/// <remarks>
/// Shapes: a number (or an enum's name) for an integer, enum or float member; true or false for a bool; an array of numbers for a vector;
/// a string for a string; a hex string for a byte block; an array for an array member; an object for a nested struct; an object with
/// <c>"$case"</c>, the arm's case value, for a union member, which must equal the selector member's value. Padding takes no value.
/// </remarks>
internal static class MessageBinder
{
    private const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<MemberInfo>> s_members = new();

    /// <summary>
    /// Set every wire member of <paramref name="target"/> from <paramref name="values"/>.
    /// </summary>
    /// <exception cref="ArgumentException">A member is missing, unknown, or given a value of the wrong shape or out of range; the message
    /// names its path.</exception>
    public static void Bind(object target, JsonObject values, string path = "")
    {
        Type type = target.GetType();
        IReadOnlyList<MemberInfo> members = WireMembers(type);
        var named = members.Where(member => member.GetCustomAttribute<PadAttribute>() is null).Select(member => member.Name).ToHashSet(StringComparer.Ordinal);
        foreach ((string key, _) in values)
        {
            if (!key.StartsWith('$') && !named.Contains(key))
                throw new ArgumentException($"{path}{key}: {type.Name} has no such wire member");
        }

        foreach (MemberInfo member in members)
        {
            if (member.GetCustomAttribute<PadAttribute>() is not null)
                continue;
            if (!values.TryGetPropertyValue(member.Name, out JsonNode? node) || node is null)
                throw new ArgumentException($"{path}{member.Name}: no value given");

            SetMember(target, member, node, path + member.Name);
        }

        foreach (MemberInfo member in members)
        {
            if (member.GetCustomAttribute<UnionAttribute>() is { } union)
                CheckSelector(target, members, member, union.Selector, path);
        }
    }

    private static void SetMember(object target, MemberInfo member, JsonNode node, string path)
    {
        Type memberType = MemberType(member);
        if (IsWireType(memberType) && CanOnlyRead(member))
        {
            // A nested struct declared { get; } = new() is filled in place.
            Bind(Read(member, target) ?? throw new ArgumentException($"{path}: the nested struct is null"), Object(node, path), path + ".");
            return;
        }

        object? value = member switch
        {
            _ when member.GetCustomAttribute<BlobAttribute>() is not null => Hex(node, path),
            _ when member.GetCustomAttribute<UnionAttribute>() is not null => Arm(member, memberType, node, path),
            _ when member.GetCustomAttribute<ArrayAttribute>() is not null => Elements(member, memberType, node, path, length: null),
            _ when member.GetCustomAttribute<FixedArrayAttribute>() is { } fixedArray => Elements(member, memberType, node, path, fixedArray.Length),
            _ => Value(member, memberType, node, path),
        };
        Write(member, target, value);
    }

    private static object? Value(MemberInfo member, Type valueType, JsonNode node, string path)
    {
        if (valueType == typeof(Vector2) || valueType == typeof(Vector3) || valueType == typeof(Quaternion))
        {
            float[] parts = [.. Array(node, path).Select((part, i) => (float)Double(part, $"{path}[{i}]"))];
            return (valueType, parts.Length) switch
            {
                (Type t, 2) when t == typeof(Vector2) => new Vector2(parts[0], parts[1]),
                (Type t, 3) when t == typeof(Vector3) => new Vector3(parts[0], parts[1], parts[2]),
                (Type t, 4) when t == typeof(Quaternion) => new Quaternion(parts[0], parts[1], parts[2], parts[3]),
                _ => throw new ArgumentException($"{path}: a {valueType.Name} takes {(valueType == typeof(Vector2) ? 2 : valueType == typeof(Vector3) ? 3 : 4)} numbers"),
            };
        }

        if (member.GetCustomAttribute<WideStringAttribute>() is not null || member.GetCustomAttribute<NarrowStringAttribute>() is not null
            || member.GetCustomAttribute<TerminatedWideStringAttribute>() is not null)
        {
            return node is JsonValue text && text.GetValueKind() == JsonValueKind.String ? text.GetValue<string>() : throw new ArgumentException($"{path}: a string");
        }

        if (IsWireType(valueType) && !valueType.IsAbstract)
        {
            object nested = Activator.CreateInstance(valueType, nonPublic: true)!;
            Bind(nested, Object(node, path), path + ".");
            return nested;
        }

        if (valueType == typeof(bool))
        {
            return node is JsonValue flag && flag.GetValueKind() is JsonValueKind.True or JsonValueKind.False
                ? flag.GetValue<bool>()
                : throw new ArgumentException($"{path}: true or false");
        }

        if (valueType == typeof(float) || valueType == typeof(double))
        {
            double number = Double(node, path);
            return valueType == typeof(float) ? (object)(float)number : number;
        }

        if (valueType.IsEnum)
        {
            if (node is JsonValue name && name.GetValueKind() == JsonValueKind.String)
            {
                return Enum.TryParse(valueType, name.GetValue<string>(), ignoreCase: false, out object? parsed)
                    ? parsed
                    : throw new ArgumentException($"{path}: {valueType.Name} has no value {name.GetValue<string>()}");
            }

            return Enum.ToObject(valueType, Integer(Enum.GetUnderlyingType(valueType), node, path));
        }

        if (valueType.IsPrimitive)
            return Integer(valueType, node, path);

        throw new ArgumentException($"{path}: no way to set a {valueType.Name}");
    }

    private static object Arm(MemberInfo member, Type unionType, JsonNode node, string path)
    {
        JsonObject values = Object(node, path);
        if (values["$case"] is not JsonValue caseValue || caseValue.GetValueKind() != JsonValueKind.Number)
            throw new ArgumentException($"{path}: a union takes an object with \"$case\", the arm's case value");

        decimal wanted = decimal.Parse(caseValue.ToJsonString(), CultureInfo.InvariantCulture);
        Type arm = Arms(member, unionType)
            .FirstOrDefault(candidate => CaseNumber(candidate.GetCustomAttribute<UnionCaseAttribute>()!.Value) == wanted)
            ?? throw new ArgumentException($"{path}: {unionType.Name} has no arm for case {wanted}");
        object instance = Activator.CreateInstance(arm, nonPublic: true)!;
        Bind(instance, values, path + ".");
        return instance;
    }

    /// <summary>
    /// The arms of the union <paramref name="member"/> holds: every concrete type in its declaring assembly that implements
    /// <paramref name="unionType"/> and carries a <see cref="UnionCaseAttribute"/>.
    /// </summary>
    internal static IEnumerable<Type> Arms(MemberInfo member, Type unionType) =>
        member.DeclaringType!.Assembly.GetTypes()
            .Where(candidate => !candidate.IsAbstract && unionType.IsAssignableFrom(candidate)
                && candidate.GetCustomAttribute<UnionCaseAttribute>() is not null);

    private static void CheckSelector(object target, IReadOnlyList<MemberInfo> members, MemberInfo union, string selector, string path)
    {
        MemberInfo? selectorMember = members.FirstOrDefault(member => member.Name == selector);
        object? arm = Read(union, target);
        if (selectorMember is null || arm?.GetType().GetCustomAttribute<UnionCaseAttribute>() is not { } unionCase)
            return;

        // An enum or a bool converts to its number as well as an integer does.
        decimal value = Convert.ToDecimal(Read(selectorMember, target), CultureInfo.InvariantCulture);
        if (value != CaseNumber(unionCase.Value))
            throw new ArgumentException($"{path}{union.Name}: its case {CaseNumber(unionCase.Value)} is not {path}{selector}'s value {value}");
    }

    private static object Elements(MemberInfo member, Type memberType, JsonNode node, string path, int? length)
    {
        Type element = memberType.IsArray ? memberType.GetElementType()! : memberType.GetGenericArguments()[0];
        List<JsonNode?> items = Array(node, path);
        if (length is { } fixedLength && items.Count != fixedLength)
            throw new ArgumentException($"{path}: exactly {fixedLength} elements");

        var values = System.Array.CreateInstance(element, items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            JsonNode item = items[i] ?? throw new ArgumentException($"{path}[{i}]: null");
            values.SetValue(Value(member, element, item, $"{path}[{i}]"), i);
        }

        if (memberType.IsArray)
            return values;

        var list = (System.Collections.IList)Activator.CreateInstance(memberType)!;
        foreach (object? value in values)
            list.Add(value);
        return list;
    }

    private static object Integer(Type type, JsonNode node, string path)
    {
        if (node is not JsonValue value || value.GetValueKind() != JsonValueKind.Number)
            throw new ArgumentException($"{path}: a number");

        decimal number = decimal.Parse(value.ToJsonString(), NumberStyles.Float, CultureInfo.InvariantCulture);
        if (number != decimal.Truncate(number))
            throw new ArgumentException($"{path}: a whole number");

        try
        {
            return Convert.ChangeType(number, type, CultureInfo.InvariantCulture);
        }
        catch (OverflowException)
        {
            throw new ArgumentException($"{path}: {number} does not fit a {type.Name}");
        }
    }

    private static double Double(JsonNode? node, string path) =>
        node is JsonValue value && value.GetValueKind() == JsonValueKind.Number
            ? double.Parse(value.ToJsonString(), NumberStyles.Float, CultureInfo.InvariantCulture)
            : throw new ArgumentException($"{path}: a number");

    private static byte[] Hex(JsonNode node, string path)
    {
        try
        {
            return node is JsonValue value && value.GetValueKind() == JsonValueKind.String
                ? Convert.FromHexString(value.GetValue<string>())
                : throw new ArgumentException($"{path}: a hex string");
        }
        catch (FormatException)
        {
            throw new ArgumentException($"{path}: a hex string");
        }
    }

    private static JsonObject Object(JsonNode node, string path) =>
        node as JsonObject ?? throw new ArgumentException($"{path}: an object");

    private static List<JsonNode?> Array(JsonNode node, string path) =>
        node is JsonArray array ? [.. array] : throw new ArgumentException($"{path}: an array");

    private static decimal CaseNumber(object value) => Convert.ToDecimal(value, CultureInfo.InvariantCulture);

    // The generator's member order, base type first: LayoutRowsTests holds it to every fixed message's generated widths, as
    // LayoutWalkTests holds the protocol reference's copy of the rule to every generated tree. Read once per type: the tap
    // calls it for every element of every list a received message holds, on the session's thread.
    internal static IReadOnlyList<MemberInfo> WireMembers(Type type) => s_members.GetOrAdd(type, ReadWireMembers);

    private static IReadOnlyList<MemberInfo> ReadWireMembers(Type type)
    {
        List<MemberInfo> members = type.BaseType is { } baseType && IsWireType(baseType) ? [.. WireMembers(baseType)] : [];
        List<MemberInfo> properties = [.. type.GetProperties(Declared)
            .Where(property => property.GetIndexParameters().Length == 0 && !property.Name.Contains('.', StringComparison.Ordinal))
            .Where(property => property.GetCustomAttribute<NotOnWireAttribute>() is null)
            .OrderBy(property => property.MetadataToken)];
        List<MemberInfo> fields = [.. type.GetFields(Declared)
            .Where(field => field.GetCustomAttribute<CompilerGeneratedAttribute>() is null && field.GetCustomAttribute<NotOnWireAttribute>() is null)
            .OrderBy(field => field.MetadataToken)];

        // Properties and fields sit in separate metadata tables, so their tokens cannot order one against the other.
        if (properties.Count > 0 && fields.Count > 0)
            throw new InvalidOperationException($"{type.Name} puts both fields and properties on the wire; their order cannot be read back by reflection");
        members.AddRange(properties.Count > 0 ? properties : fields);
        return members;
    }

    internal static bool IsWireType(Type type) =>
        type.GetCustomAttribute<WireStructAttribute>() is not null || type.GetCustomAttribute<WireMessageAttribute>() is not null;

    internal static Type MemberType(MemberInfo member) => member switch
    {
        PropertyInfo property => property.PropertyType,
        FieldInfo field => field.FieldType,
        _ => throw new ArgumentException($"{member.Name} is not a property or a field"),
    };

    private static bool CanOnlyRead(MemberInfo member) => member is PropertyInfo { SetMethod: null };

    internal static object? Read(MemberInfo member, object target) => member switch
    {
        PropertyInfo property => property.GetValue(target),
        FieldInfo field => field.GetValue(target),
        _ => null,
    };

    private static void Write(MemberInfo member, object target, object? value)
    {
        switch (member)
        {
            case PropertyInfo { SetMethod: not null } property:
                property.SetValue(target, value);
                break;
            case FieldInfo field:
                field.SetValue(target, value);
                break;

            // A list declared { get; } = [] is filled in place, as a nested struct is; a fixed array keeps its length.
            case PropertyInfo property when value is IList given && property.GetValue(target) is IList held:
                if (held.IsFixedSize)
                {
                    for (int i = 0; i < given.Count; i++)
                        held[i] = given[i];
                    break;
                }

                held.Clear();
                foreach (object? item in given)
                    held.Add(item);
                break;
            default:
                throw new ArgumentException($"{member.Name}: the member cannot be set");
        }
    }
}
