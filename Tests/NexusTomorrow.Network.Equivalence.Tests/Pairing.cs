using System.Collections;
using System.Reflection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// How a declared type pairs with its legacy twin: the twin resolver, the union flattening every pairing follows, and the pinned rules for the legacy
/// shapes member names cannot follow. Rules are keyed by the declared type's full name (nested types use '+') and each carries the fact that makes it
/// necessary, so none passes unreviewed.
/// </summary>
internal sealed record Pairing(Func<Type, Type?> TwinOf)
{
    private static readonly Dictionary<string, string> s_noReasons = [];
    private static readonly Dictionary<string, RowColumns> s_noRows = [];
    private static readonly Dictionary<string, Rule> s_noRules = [];
    private static readonly Dictionary<(string Type, string Member), Rename> s_noRenames = [];

    /// <summary>
    /// Union arms the filler never picks: each is excluded or checked one way on its own, so the unions holding it are checked over their other arms.
    /// </summary>
    public Func<Type, bool> SkipsArm { get; init; } = _ => false;

    /// <summary>
    /// Types checked against the legacy Read only, with the reason: their legacy Write puts different bits on the wire than their legacy Read takes.
    /// </summary>
    public IReadOnlyDictionary<string, string> ReadOnly { get; init; } = s_noReasons;

    /// <summary>
    /// Types whose column lists the legacy model keeps as one list of records.
    /// </summary>
    public IReadOnlyDictionary<string, RowColumns> Rows { get; init; } = s_noRows;

    /// <summary>
    /// Adjustments to every filled instance of a type that keep its values where the legacy model carries them unchanged.
    /// </summary>
    public IReadOnlyDictionary<string, Rule> Limits { get; init; } = s_noRules;

    /// <summary>
    /// Off-wire legacy members to set on the legacy instance a declared instance of the type maps onto, and on the legacy instance a read check of the
    /// type reads into.
    /// </summary>
    public IReadOnlyDictionary<string, Rule> Presets { get; init; } = s_noRules;

    /// <summary>
    /// Declared members whose legacy twin has another name, keyed by the declaring type's full name and the member's name.
    /// </summary>
    public IReadOnlyDictionary<(string Type, string Member), Rename> Renames { get; init; } = s_noRenames;

    public bool IsReadOnly(Type declared)
    {
        return declared.FullName is { } fullName && ReadOnly.ContainsKey(fullName);
    }

    public RowColumns? RowsOf(Type declared)
    {
        return declared.FullName is { } fullName && Rows.TryGetValue(fullName, out RowColumns? rows) ? rows : null;
    }

    /// <summary>
    /// The name of the legacy member that twins <paramref name="member"/>.
    /// </summary>
    public string LegacyNameOf(WireMember member)
    {
        return member.Member.DeclaringType?.FullName is { } typeName && Renames.TryGetValue((typeName, member.Name), out Rename? rename) ? rename.LegacyMember : member.Name;
    }

    /// <summary>
    /// Apply each limit to every instance of its type in the graph under <paramref name="root"/>.
    /// </summary>
    public void Limit(object root)
    {
        if (Limits.Count > 0)
            Visit(root, instance => Apply(Limits, instance.GetType(), instance));
    }

    public void Preset(Type declared, object legacy)
    {
        Apply(Presets, declared, legacy);
    }

    /// <summary>
    /// The legacy member that holds <paramref name="armTwin"/> for the union named <paramref name="unionName"/>, where the legacy model keeps one member per
    /// arm (KillerPlayer and KillerCreature for the union Killer): a public property whose name starts with the union's name and whose type is the arm's twin.
    /// </summary>
    public static PropertyInfo? ArmSlotOf(Type legacyHolder, string unionName, Type armTwin)
    {
        return Array.Find(legacyHolder.GetProperties(BindingFlags.Instance | BindingFlags.Public),
            property => property.Name.StartsWith(unionName, StringComparison.Ordinal) && property.PropertyType == armTwin);
    }

    private static void Apply(IReadOnlyDictionary<string, Rule> rules, Type declared, object target)
    {
        if (declared.FullName is { } fullName && rules.TryGetValue(fullName, out Rule? rule))
            rule.Apply(target);
    }

    private static void Visit(object instance, Action<object> visit)
    {
        visit(instance);
        foreach (WireMember member in WireMembers.Of(instance.GetType()))
        {
            object? value = MemberAccess.GetValue(member.Member, instance);
            switch (member.Shape)
            {
                case WireShape.Union when value is not null:
                case WireShape.Value when member.Encoding == WireEncoding.Struct && value is not null:
                    Visit(value, visit);
                    break;
                case WireShape.Array or WireShape.FixedArray when member.Encoding == WireEncoding.Struct && value is IEnumerable elements:
                    foreach (object? element in elements)
                    {
                        if (element is not null)
                            Visit(element, visit);
                    }

                    break;
            }
        }
    }
}

/// <summary>
/// Column lists of a declared type that the legacy model keeps as one list of records: element i of column <c>Column</c> is field <c>Field</c> of record i
/// in the legacy list <see cref="LegacyList"/>.
/// </summary>
internal sealed record RowColumns(string LegacyList, IReadOnlyList<(string Column, string Field)> Columns, string Reason)
{
    public bool Holds(string member)
    {
        return Columns.Any(column => column.Column == member);
    }
}

internal sealed record Rule(string Reason, Action<object> Apply);

internal sealed record Rename(string LegacyMember, string Reason);
