using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Nodes;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Tools;

/// <summary>
/// A message's wire members as JSON, in the shapes <see cref="MessageBinder"/> takes, so a dumped message binds back to the same bytes:
/// a number for an integer (an enum as its number) or a float, true or false, an array of numbers for a vector, a string, a hex string
/// for a byte block, an array for an array member, an object for a nested struct, and an object with <c>"$case"</c> for a union arm.
/// Padding is left out. A float that is not finite is written as its name (<c>"NaN"</c>, <c>"Infinity"</c>), which JSON has no
/// number for and the binder refuses.
/// </summary>
internal static class MessageDump
{
    public static JsonObject Of(object message)
    {
        var values = new JsonObject();
        foreach (MemberInfo member in MessageBinder.WireMembers(message.GetType()))
        {
            if (member.GetCustomAttribute<PadAttribute>() is null)
                values[member.Name] = Member(member, MessageBinder.Read(member, message));
        }

        return values;
    }

    private static JsonNode? Member(MemberInfo member, object? value) => member switch
    {
        _ when value is null => null,
        _ when member.GetCustomAttribute<BlobAttribute>() is not null => Convert.ToHexString((byte[])value),
        _ when member.GetCustomAttribute<UnionAttribute>() is not null => Arm(value),
        _ when member.GetCustomAttribute<ArrayAttribute>() is not null || member.GetCustomAttribute<FixedArrayAttribute>() is not null
            => new JsonArray([.. ((IEnumerable)value).Cast<object?>().Select(Value)]),
        _ => Value(value),
    };

    private static JsonObject Arm(object arm)
    {
        UnionCaseAttribute unionCase = arm.GetType().GetCustomAttribute<UnionCaseAttribute>()
            ?? throw new InvalidOperationException($"{arm.GetType().Name} is a union arm with no case");
        var values = new JsonObject { ["$case"] = Convert.ToDecimal(unionCase.Value, CultureInfo.InvariantCulture) };
        foreach ((string key, JsonNode? node) in Of(arm))
            values[key] = node?.DeepClone();
        return values;
    }

    private static JsonNode? Value(object? value) => value switch
    {
        null => null,
        bool flag => flag,
        float number => Number(number),
        double number => Number(number),
        string text => text,
        Vector2 v => new JsonArray(Number(v.X), Number(v.Y)),
        Vector3 v => new JsonArray(Number(v.X), Number(v.Y), Number(v.Z)),
        Quaternion q => new JsonArray(Number(q.X), Number(q.Y), Number(q.Z), Number(q.W)),
        Enum or sbyte or byte or short or ushort or int or uint or long or ulong => Convert.ToDecimal(value, CultureInfo.InvariantCulture),
        _ when MessageBinder.IsWireType(value.GetType()) => Of(value),
        _ => throw new InvalidOperationException($"no JSON shape for a {value.GetType().Name}"),
    };

    private static JsonValue Number(double number) =>
        double.IsFinite(number) ? JsonValue.Create(number) : JsonValue.Create(number.ToString(CultureInfo.InvariantCulture));

    private static JsonValue Number(float number) =>
        float.IsFinite(number) ? JsonValue.Create(number) : JsonValue.Create(number.ToString(CultureInfo.InvariantCulture));
}
