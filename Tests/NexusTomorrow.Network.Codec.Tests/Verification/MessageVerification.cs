using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

/// <summary>
/// The documentation each rung requires of a message and of every layout the message reaches.
/// </summary>
internal static partial class MessageVerification
{
    /// <summary>
    /// The sentence a member's summary begins with when the member keeps its neutral name at <see cref="Rung.Fields"/>; it holds in
    /// either direction, for a value the client ignores on read or writes without meaning.
    /// </summary>
    public const string GivenNoMeaning = "The client gives this value no meaning.";

    public static Rung? RungOf(Type message) => message.GetCustomAttribute<VerifiedAttribute>()?.Rung;

    public static List<string> Problems(Type message, string key, IReadOnlyDictionary<string, string> summaries, IReadOnlyCollection<Assembly> models)
    {
        var problems = new List<string>();
        if (RungOf(message) is not { } rung)
            return problems;
        if (NeutralMessageName().IsMatch(message.Name))
            problems.Add($"{key}: {rung} needs a name of its own on {message.Name}");
        if (!summaries.ContainsKey("T:" + DocName(message)))
            problems.Add($"{key}: {rung} needs a summary on {message.Name}");
        if (rung < Rung.Fields)
            return problems;

        foreach (Type type in Reachable(message, models))
        {
            foreach (MemberInfo member in WireMembers(type))
            {
                string id = (member is PropertyInfo ? "P:" : "F:") + DocName(type) + "." + member.Name;
                if (!summaries.TryGetValue(id, out string? summary))
                    problems.Add($"{key}: {rung} needs a summary on {type.Name}.{member.Name}");
                else if (NeutralName().IsMatch(member.Name) && !summary.StartsWith(GivenNoMeaning, StringComparison.Ordinal))
                    problems.Add($"{key}: {type.Name}.{member.Name} keeps a neutral name, so its summary must begin with the no-meaning sentence");
            }
        }

        return problems;
    }

    private static string DocName(Type type) => type.FullName!.Replace('+', '.');

    // Any accessibility, as the generator takes it; an auto-property's compiler-generated backing field is not a member of its own.
    private static IEnumerable<MemberInfo> WireMembers(Type type) =>
        type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => m is PropertyInfo or FieldInfo
                && !m.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
                && m.GetCustomAttribute<NotOnWireAttribute>() is null);

    private static Type TypeOf(MemberInfo member) => member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;

    private static Type ElementOf(Type type) => type.IsArray ? type.GetElementType()! : type.IsGenericType ? type.GetGenericArguments()[0] : type;

    private static bool IsLayout(Type type) => type.GetCustomAttribute<WireStructAttribute>() is not null || type.GetCustomAttribute<WireMessageAttribute>() is not null;

    // Arms are found in every model assembly, since a union's arms need not live beside the message that holds it.
    private static IEnumerable<Type> Reachable(Type root, IReadOnlyCollection<Assembly> models)
    {
        var seen = new HashSet<Type>();
        var pending = new Stack<Type>([root]);
        while (pending.TryPop(out Type? type))
        {
            if (!seen.Add(type))
                continue;
            yield return type;
            if (type.BaseType is { } baseType && IsLayout(baseType))
                pending.Push(baseType);
            foreach (MemberInfo member in WireMembers(type))
            {
                Type memberType = TypeOf(member);
                if (member.GetCustomAttribute<UnionAttribute>() is not null)
                {
                    foreach (Type arm in models.SelectMany(a => a.GetTypes()).Where(t => memberType.IsAssignableFrom(t) && t.GetCustomAttribute<UnionCaseAttribute>() is not null))
                        pending.Push(arm);
                }
                else if (IsLayout(ElementOf(memberType)))
                {
                    pending.Push(ElementOf(memberType));
                }
            }
        }
    }

    [GeneratedRegex("^Unknown[0-9]+$")]
    private static partial Regex NeutralName();

    [GeneratedRegex("^(Client|Server|Message)[0-9A-F]{4}$")]
    private static partial Regex NeutralMessageName();
}
