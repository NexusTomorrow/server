using System.Collections.Immutable;
using System.Globalization;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Compares a declared layout tree with the client's, and names each disagreement by its path: <c>[4]</c> is node 4 of a sequence,
/// <c>.element</c> enters an array's element and <c>.arm 0x02</c> a union's arm for selector 2. Two trees agree when they put the same bits on
/// every path: their strings, arrays, unions, blobs, counts and selectors sit at the same places with the same widths, and between them their
/// other fields take the same bits, where one field of one side may stand for a run of fields of the other (16 against 8 8). Node numbers,
/// in paths and in problems, are the declaration's.
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

    // Walks both sequences side by side: i is the declaration's node and j the client's, both at the same bit on every path. Two fields of one
    // width, or two nodes of one other kind, pair one to one; two fields of different widths open a split (SameBits). A pair that cannot be
    // aligned is reported at the declaration's node and ends the sequence, since nothing after it is known to sit at the same bit.
    private static void CompareSequence(string label, string path, ImmutableArray<WireNode> declared, ImmutableArray<WireNode> client, List<string> problems)
    {
        HashSet<int> declaredReferenced = Referenced(declared);
        HashSet<int> clientReferenced = Referenced(client);

        // The declaration's node each client node pairs with one to one, and -1 for a client node inside a split. A count or a selector never
        // splits, so every client node a later array or union names has its pair.
        int[] declaredIndexOf = new int[client.Length];
        Array.Fill(declaredIndexOf, -1);

        int i = 0;
        int j = 0;
        while (i < declared.Length && j < client.Length)
        {
            WireNode mine = declared[i];
            WireNode theirs = client[j];
            string here = path + "[" + i.ToString(CultureInfo.InvariantCulture) + "]";
            if (mine.Kind == WireNodeKind.Recursive || theirs.Kind == WireNodeKind.Recursive)
            {
                problems.Add($"{label} {here}: a layout that contains itself is not compared");
                return;
            }

            if (mine.Kind == WireNodeKind.Field && theirs.Kind == WireNodeKind.Field && mine.Bits != theirs.Bits)
            {
                if (!SameBits(declared, i, declaredReferenced, client, j, clientReferenced, out int declaredEnd, out int clientEnd))
                {
                    problems.Add($"{label} {here}: the client has {Describe(theirs)}, the declaration {Describe(mine)}");
                    return;
                }

                i = declaredEnd;
                j = clientEnd;
                continue;
            }

            if (mine.Kind != theirs.Kind)
            {
                problems.Add($"{label} {here}: the client has {Describe(theirs)}, the declaration {Describe(mine)}");
                return;
            }

            declaredIndexOf[j] = i;
            switch (mine.Kind)
            {
                case WireNodeKind.Array:
                    if (mine.Reference != declaredIndexOf[theirs.Reference])
                    {
                        problems.Add($"{label} {here}: the client counts it with node {declaredIndexOf[theirs.Reference]}, the declaration with node {mine.Reference}");
                        return;
                    }

                    CompareSequence(label, here + ".element", mine.Element, theirs.Element, problems);
                    break;
                case WireNodeKind.Union:
                    if (mine.Reference != declaredIndexOf[theirs.Reference])
                    {
                        problems.Add($"{label} {here}: the client selects it with node {declaredIndexOf[theirs.Reference]}, the declaration with node {mine.Reference}");
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

            i++;
            j++;
        }

        // The walk stops when either side ends, so at most one side has nodes left: the problem names that side and counts them, since a split
        // that used more nodes on one side makes the two sequences' lengths say nothing about which goes on.
        string where = path.Length == 0 ? label : label + " " + path;
        if (j < client.Length)
            problems.Add($"{where}: the client's sequence goes on for {client.Length - j} nodes after the declaration's ends");
        else if (i < declared.Length)
            problems.Add($"{where}: the declaration's sequence goes on for {declared.Length - i} nodes after the client's ends");
    }

    // Two fields of different widths at the same bit open a split: each side takes in its next field while its total is the smaller, until the
    // totals meet, and declaredEnd and clientEnd are the nodes after each run. The split fails when a run would take in a node that is not a
    // field or a field some later array or union names, since those must pair one to one; when a sequence ends first; and when both runs hold
    // more than one field, since then a field of each side holds part of a field of the other (8 16 against 16 8) and the bits mean other things
    // on each side though the totals meet. One field against a run is the same bits split another way, which only reading the fields can settle.
    private static bool SameBits(
        ImmutableArray<WireNode> declared, int i, HashSet<int> declaredReferenced, ImmutableArray<WireNode> client, int j, HashSet<int> clientReferenced,
        out int declaredEnd, out int clientEnd)
    {
        declaredEnd = i;
        clientEnd = j;
        int declaredBits = 0;
        int clientBits = 0;
        do
        {
            if (declaredBits <= clientBits)
            {
                if (!Splittable(declared, declaredEnd, declaredReferenced))
                    return false;
                declaredBits += declared[declaredEnd++].Bits;
            }
            else
            {
                if (!Splittable(client, clientEnd, clientReferenced))
                    return false;
                clientBits += client[clientEnd++].Bits;
            }
        }
        while (declaredBits != clientBits);

        return declaredEnd - i == 1 || clientEnd - j == 1;
    }

    private static bool Splittable(ImmutableArray<WireNode> nodes, int index, HashSet<int> referenced)
    {
        return index < nodes.Length && nodes[index].Kind == WireNodeKind.Field && !referenced.Contains(index);
    }

    // The nodes a later array counts with or a later union selects with; a count or a selector comes before what it governs
    // (WireTree.ParseReference), so every reference names an earlier node.
    private static HashSet<int> Referenced(ImmutableArray<WireNode> nodes)
    {
        return [.. nodes.Where(node => node.Kind is WireNodeKind.Array or WireNodeKind.Union).Select(node => node.Reference)];
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
    /// elements together; the fields and strings every arm of a union ends with alike leave the arms and follow the union. Each sequence's
    /// references are renumbered when nodes merge or leave a union.
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
            int shared = node.Kind == WireNodeKind.Union ? SharedTail(node.Arms) : 0;
            if (shared == 0)
            {
                merged.Add(node);
            }
            else
            {
                merged.Add(WireNode.Union(node.Reference, [.. node.Arms.Select(arm => new WireArm(arm.Selector, arm.Nodes[..^shared]))]));
                merged.AddRange(node.Arms[0].Nodes[^shared..]);
            }
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

    // How many nodes at the end of every arm are the same field or string in each. Those refer to no other node, and no node refers to them,
    // since a count or a selector comes before what it governs (WireTree.ParseReference), so they read the same bits after the union as at
    // the end of each arm.
    private static int SharedTail(ImmutableArray<WireArm> arms)
    {
        if (arms.IsEmpty)
            return 0;

        int shared = 0;
        while (true)
        {
            int back = shared + 1;
            ImmutableArray<WireNode> first = arms[0].Nodes;
            if (first.Length < back || first[^back].Kind is not (WireNodeKind.Field or WireNodeKind.WideString or WireNodeKind.NarrowString or WireNodeKind.TerminatedWideString))
                return shared;

            WireNode last = first[^back];
            foreach (WireArm arm in arms)
            {
                if (arm.Nodes.Length < back || arm.Nodes[^back].Kind != last.Kind || arm.Nodes[^back].Bits != last.Bits)
                    return shared;
            }

            shared = back;
        }
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
