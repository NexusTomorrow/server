using System.Reflection;
using System.Text.RegularExpressions;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

/// <summary>
/// The documentation each rung requires of a message and of every layout the message reaches.
/// </summary>
internal static partial class MessageVerification
{
    /// <summary>
    /// The sentence a member's summary begins with when the member keeps its neutral name at <see cref="Rung.Fields"/>.
    /// </summary>
    public const string IgnoredByTheClient = "The client reads this value and ignores it.";

    public static Rung? RungOf(Type message) => message.GetCustomAttribute<VerifiedAttribute>()?.Rung;

    public static List<string> Problems(Type message, string key, IReadOnlyDictionary<string, string> summaries)
    {
        var problems = new List<string>();
        if (RungOf(message) is not { } rung)
            return problems;
        if (!summaries.ContainsKey("T:" + DocName(message)))
            problems.Add($"{key}: {rung} needs a summary on {message.Name}");
        if (rung < Rung.Fields)
            return problems;

        foreach (Type type in Reachable(message))
        {
            foreach (MemberInfo member in WireMembers(type))
            {
                string id = (member is PropertyInfo ? "P:" : "F:") + DocName(type) + "." + member.Name;
                if (!summaries.TryGetValue(id, out string? summary))
                    problems.Add($"{key}: {rung} needs a summary on {type.Name}.{member.Name}");
                else if (NeutralName().IsMatch(member.Name) && !summary.StartsWith(IgnoredByTheClient, StringComparison.Ordinal))
                    problems.Add($"{key}: {type.Name}.{member.Name} keeps a neutral name, so its summary must begin with the ignored-by-the-client sentence");
            }
        }

        return problems;
    }

    private static string DocName(Type type) => type.FullName!.Replace('+', '.');

    private static IEnumerable<MemberInfo> WireMembers(Type type) =>
        type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => m is PropertyInfo or FieldInfo && m.GetCustomAttribute<NotOnWireAttribute>() is null);

    private static Type TypeOf(MemberInfo member) => member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;

    private static Type ElementOf(Type type) => type.IsArray ? type.GetElementType()! : type.IsGenericType ? type.GetGenericArguments()[0] : type;

    private static bool IsLayout(Type type) => type.GetCustomAttribute<WireStructAttribute>() is not null || type.GetCustomAttribute<WireMessageAttribute>() is not null;

    private static IEnumerable<Type> Reachable(Type root)
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
                    foreach (Type arm in memberType.Assembly.GetTypes().Where(t => memberType.IsAssignableFrom(t) && t.GetCustomAttribute<UnionCaseAttribute>() is not null))
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
}
