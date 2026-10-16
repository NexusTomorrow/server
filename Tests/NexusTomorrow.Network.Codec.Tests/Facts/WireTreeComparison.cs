using System.Collections.Immutable;
using System.Globalization;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Compares a declared layout tree with the client's node by node, and names each disagreement by its path: <c>[4]</c> is node 4 of a
/// sequence, <c>.element</c> enters an array's element and <c>.arm 0x02</c> a union's arm for selector 2.
/// </summary>
internal static class WireTreeComparison
{
    public static List<string> Compare(string label, ImmutableArray<WireNode> declared, ImmutableArray<WireNode> client)
    {
        var problems = new List<string>();
        CompareSequence(label, "", Normalize(declared), Normalize(client), problems);
        return problems;
    }

    /// <summary>
    /// The selector values at which the client facts give a union's arms: every value of a selector up to 8 bits wide, else 0, 1, 2, 3,
    /// 0xFF, 0x100, the top bit alone and all ones. Null means every value.
    /// </summary>
    public static HashSet<ulong>? KnownSelectorValues(int width)
    {
        if (width <= 8)
            return null;
        ulong top = width >= 64 ? ulong.MaxValue : (1UL << width) - 1;
        return [0, 1, 2, 3, 0xFF, 0x100, 1UL << (width - 1), top];
    }

    private static void CompareSequence(string label, string path, ImmutableArray<WireNode> declared, ImmutableArray<WireNode> client, List<string> problems)
    {
        for (int i = 0; i < Math.Min(declared.Length, client.Length); i++)
        {
            WireNode mine = declared[i];
            WireNode theirs = client[i];
            string here = path + "[" + i.ToString(CultureInfo.InvariantCulture) + "]";
            if (mine.Kind == WireNodeKind.Recursive || theirs.Kind == WireNodeKind.Recursive)
            {
                problems.Add($"{label} {here}: a layout that contains itself is not compared");
                return;
            }

            if (mine.Kind != theirs.Kind || (mine.Kind == WireNodeKind.Field && mine.Bits != theirs.Bits))
            {
                problems.Add($"{label} {here}: the client has {Describe(theirs)}, the declaration {Describe(mine)}");
                return;
            }

            switch (mine.Kind)
            {
                case WireNodeKind.Array:
                    if (mine.Reference != theirs.Reference)
                    {
                        problems.Add($"{label} {here}: the client counts it with node {theirs.Reference}, the declaration with node {mine.Reference}");
                        return;
                    }

                    CompareSequence(label, here + ".element", mine.Element, theirs.Element, problems);
                    break;
                case WireNodeKind.Union:
                    if (mine.Reference != theirs.Reference)
                    {
                        problems.Add($"{label} {here}: the client selects it with node {theirs.Reference}, the declaration with node {mine.Reference}");
                        return;
                    }

                    CompareArms(label, here, mine, theirs, declared[mine.Reference].Bits, problems);
                    break;

                // The client facts do not give whether a blob starts on a byte boundary, so only its length's width is compared.
                case WireNodeKind.Blob:
                    if (mine.Bits != theirs.Bits)
                        problems.Add($"{label} {here}: the client's blob length is {theirs.Bits} bits, the declaration's {mine.Bits}");
                    break;
            }
        }

        if (declared.Length != client.Length)
            problems.Add($"{(path.Length == 0 ? label : label + " " + path)}: the client's sequence has {client.Length} nodes, the declaration's {declared.Length}");
    }

    private static void CompareArms(string label, string here, WireNode mine, WireNode theirs, int selectorBits, List<string> problems)
    {
        var declaredArms = mine.Arms.ToDictionary(arm => arm.Selector);
        var clientArms = theirs.Arms.ToDictionary(arm => arm.Selector);
        HashSet<ulong>? known = KnownSelectorValues(selectorBits);
        var emptyRun = new List<ulong>();

        void FlushEmptyRun()
        {
            if (emptyRun.Count == 1)
                problems.Add($"{label} {here}: the client accepts selector {emptyRun[0]} and the declaration has no arm");
            else if (emptyRun.Count > 1)
                problems.Add($"{label} {here}: the client reads selectors {emptyRun[0]} to {emptyRun[^1]} with nothing after them, and the declaration has no arm for them");
            emptyRun.Clear();
        }

        foreach (ulong value in declaredArms.Keys.Union(clientArms.Keys).Order())
        {
            if (known is not null && !known.Contains(value))
                continue;

            bool declaredHas = declaredArms.TryGetValue(value, out WireArm? declaredArm);
            bool clientHas = clientArms.TryGetValue(value, out WireArm? clientArm);
            if (clientHas && !declaredHas && clientArm!.Nodes.IsEmpty && (emptyRun.Count == 0 || emptyRun[^1] + 1 == value))
            {
                emptyRun.Add(value);
                continue;
            }

            FlushEmptyRun();
            if (clientHas && !declaredHas)
            {
                if (clientArm!.Nodes.IsEmpty)
                    emptyRun.Add(value);
                else
                    problems.Add($"{label} {here}: the client accepts selector {value} and the declaration has no arm");
            }
            else if (declaredHas && !clientHas)
            {
                problems.Add($"{label} {here}: the declaration has an arm for selector {value}, which the client does not accept");
            }
            else
            {
                CompareSequence(label, $"{here}.arm 0x{value:X2}", declaredArm!.Nodes, clientArm!.Nodes, problems);
            }
        }

        FlushEmptyRun();
    }

    /// <summary>
    /// Rewrite the forms the wire cannot tell apart into one form, on both sides alike: an array counted by a 1-bit field becomes the union
    /// {0=(), 1=(element)}; consecutive arrays sharing one count whose elements are all fields of one width become one array of their
    /// elements together. Each sequence's references are renumbered when nodes merge.
    /// </summary>
    private static ImmutableArray<WireNode> Normalize(ImmutableArray<WireNode> nodes)
    {
        var merged = new List<WireNode>();
        int[] newIndex = new int[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            WireNode node = Inner(nodes[i]);
            if (node.Kind == WireNodeKind.Array && merged.Count > 0 && merged[^1].Kind == WireNodeKind.Array && nodes[i - 1].Kind == WireNodeKind.Array
                && nodes[i - 1].Reference == node.Reference && OneWidth(merged[^1].Element, node.Element))
            {
                merged[^1] = WireNode.Array(merged[^1].Reference, [.. merged[^1].Element, .. node.Element]);
                newIndex[i] = merged.Count - 1;
                continue;
            }

            newIndex[i] = merged.Count;
            merged.Add(node);
        }

        ImmutableArray<WireNode>.Builder result = ImmutableArray.CreateBuilder<WireNode>(merged.Count);
        foreach (WireNode node in merged)
        {
            WireNode renumbered = node.Kind switch
            {
                WireNodeKind.Array => WireNode.Array(newIndex[node.Reference], node.Element),
                WireNodeKind.Union => WireNode.Union(newIndex[node.Reference], node.Arms),
                _ => node,
            };
            if (renumbered.Kind == WireNodeKind.Array && result[renumbered.Reference].Bits == 1)
                renumbered = WireNode.Union(renumbered.Reference, [new WireArm(0, []), new WireArm(1, renumbered.Element)]);
            result.Add(renumbered);
        }

        return result.MoveToImmutable();
    }

    private static WireNode Inner(WireNode node)
    {
        return node.Kind switch
        {
            WireNodeKind.Array => WireNode.Array(node.Reference, Normalize(node.Element)),
            WireNodeKind.Union => WireNode.Union(node.Reference, [.. node.Arms.Select(arm => new WireArm(arm.Selector, Normalize(arm.Nodes)))]),
            _ => node,
        };
    }

    private static bool OneWidth(ImmutableArray<WireNode> first, ImmutableArray<WireNode> second)
    {
        int width = first.IsEmpty ? 0 : first[0].Bits;
        return width > 0 && first.Concat(second).All(node => node.Kind == WireNodeKind.Field && node.Bits == width);
    }

    private static string Describe(WireNode node)
    {
        return node.Kind switch
        {
            WireNodeKind.Field => node.Bits.ToString(CultureInfo.InvariantCulture),
            WireNodeKind.WideString => "a wide string",
            WireNodeKind.NarrowString => "a narrow string",
            WireNodeKind.TerminatedWideString => "a terminated wide string",
            WireNodeKind.Array => "an array",
            WireNodeKind.Union => "a union",
            WireNodeKind.Blob => "a blob",
            _ => "a layout that contains itself",
        };
    }
}
