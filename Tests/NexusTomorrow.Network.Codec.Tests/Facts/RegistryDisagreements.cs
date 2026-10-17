namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Every declared message that disagrees with the client facts, keyed by the opcode, side and path each problem names (the text before its colon,
/// for example <c>0x01C3 client to server [4].element[3]</c>), each with its reason and the problem text after the colon. The registry check fails
/// on a disagreement not listed here, on a listed one that no longer occurs and on a listed one whose problem text has changed, so the list only
/// shrinks as the reading settles each one.
/// </summary>
internal static class RegistryDisagreements
{
    private const string BitsDiffer = "unsettled: the declaration puts a different number of bits on the wire than the client takes";
    private const string FieldsDiffer = "unsettled: the bits agree, but the client takes them as fields of other widths";
    private const string PartMissing = "unsettled: the client takes a part the declaration lacks";
    private const string TreeDiffers = "unsettled: the client's full layout differs; settled when its area is read";
    private const string Unsettled = "unsettled: not yet read";

    public static IReadOnlyDictionary<string, Pin> Pinned { get; } = new Dictionary<string, Pin>(StringComparer.Ordinal)
    {
        ["0x0112 server to client [5].element[7].arm 0x03"] = new(TreeDiffers, "the client's sequence has 1 nodes, the declaration's 2"), // Server0112
        ["0x0132 server to client [7].arm 0x03"] = new(TreeDiffers, "the client's sequence has 1 nodes, the declaration's 2"), // Server0132
        ["0x0134 server to client [7].arm 0x03"] = new(TreeDiffers, "the client's sequence has 1 nodes, the declaration's 2"), // Server0134
        ["0x013C server to client [1].element[1]"] = new(TreeDiffers, "the client has 32, the declaration 3"), // Server013C
        ["0x0152 server to client [1].element[0]"] = new(TreeDiffers, "the client has 8, the declaration 3"), // Server0152
        ["0x015A client to server [1]"] = new(BitsDiffer, "the client has 7, the declaration 32"), // Client015A
        ["0x01B4 server to client [1]"] = new(TreeDiffers, "the client has 32, the declaration a blob"), // Server01B4
        ["0x01C0 server to client"] = new(BitsDiffer, "the client's sequence has 6 nodes, the declaration's 5"), // Server01C0
        ["0x01C3 client to server [4].element[3].arm 0x01[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01C3
        ["0x01C3 client to server [4].element[3].arm 0x07[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01C3
        ["0x01C8 server to client [12].element[3].arm 0x01[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Server01C8
        ["0x01C8 server to client [12].element[3].arm 0x07[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Server01C8
        ["0x01D4 client to server [4].element[3].arm 0x01[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01D4
        ["0x01D4 client to server [4].element[3].arm 0x07[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01D4
        ["0x0247 server to client [1]"] = new(TreeDiffers, "the client accepts selector 34 and the declaration has no arm"), // Server0247
        ["0x0247 server to client [1].arm 0x05[5]"] = new(TreeDiffers, "the client has 32, the declaration 1"), // Server0247
        ["0x0247 server to client [1].arm 0x0B"] = new(TreeDiffers, "the client's sequence has 7 nodes, the declaration's 3"), // Server0247
        ["0x0247 server to client [1].arm 0x0F"] = new(TreeDiffers, "the client's sequence has 13 nodes, the declaration's 9"), // Server0247
        ["0x0247 server to client [1].arm 0x23"] = new(TreeDiffers, "the client's sequence has 5 nodes, the declaration's 1"), // Server0247
        ["0x033F client to server [1]"] = new("unsettled: the declaration puts a different number of bits on the wire than the client takes, and lacks the client's wide string", "the client has 32, the declaration a union"), // Client033F
        ["0x0359 server to client [2].element[1]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server0359
        ["0x0359 server to client [2].element[1].arm 0x01[0]"] = new(TreeDiffers, "the client has a narrow string, the declaration a wide string"), // Server0359
        ["0x035F server to client [5].element[5]"] = new(TreeDiffers, "the client has a union, the declaration an array"), // Server035F
        ["0x046F server to client"] = new(BitsDiffer, "the client's sequence has 32 nodes, the declaration's 33"), // Server046F
        ["0x047A server to client [4].element"] = new(TreeDiffers, "the client's sequence has 30 nodes, the declaration's 31"), // Server047A
        ["0x0489 server to client [4].element[0]"] = new(TreeDiffers, "the client has 64, the declaration 14"), // Server0489
        ["0x0598 server to client [3]"] = new(TreeDiffers, "the client has 16, the declaration a terminated wide string"), // Server0598
        ["0x0599 server to client [3]"] = new(FieldsDiffer, "the client has 16, the declaration a terminated wide string"), // Server0599
        ["0x05E7 server to client [1]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server05E7
        ["0x05E7 server to client [3]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server05E7
        ["0x060A server to client [2].element[6]"] = new(TreeDiffers, "the client has 2, the declaration 14"), // Server060A
        ["0x0738 server to client [3].element[0]"] = new(TreeDiffers, "the client has 3, the declaration 32"), // Server0738
        ["0x07DC client to server [6].element[1].arm 0x04[1]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Client07DC
        ["0x0823 client to server [1]"] = new(PartMissing, "the client has 32, the declaration a blob"), // Client0823
        ["0x082A client to server [0]"] = new(BitsDiffer, "the client has 32, the declaration 20"), // Client082A
        ["0x084C server to client [1].element"] = new(TreeDiffers, "the client's sequence has 6 nodes, the declaration's 7"), // Server084C
        ["0x092C server to client [1].element[3]"] = new(TreeDiffers, "the client has 32, the declaration a union"), // Server092C
        ["0x0954 server to client"] = new(BitsDiffer, "the client's sequence has 8 nodes, the declaration's 4"), // Server0954
        ["0x0967 server to client"] = new(BitsDiffer, "the client's sequence has 4 nodes, the declaration's 5"), // Server0967
        ["0x0978 server to client [1].element[1]"] = new(TreeDiffers, "the client has 1, the declaration 32"), // Server0978
        ["0x098B server to client [1].element[6].element[11].element[1]"] = new(Unsettled, "the client accepts selector 3 and the declaration has no arm"), // Server098B
        ["0x098B server to client [1].element[6].element[11].element[1].arm 0x01"] = new(Unsettled, "the client's sequence has 2 nodes, the declaration's 1"), // Server098B
        ["0x098B server to client [1].element[6].element[11].element[1].arm 0x02"] = new(Unsettled, "the client's sequence has 1 nodes, the declaration's 0"), // Server098B
    };
}

/// <summary>
/// One pinned disagreement: why it stands, and the problem text the check reports for it, several joined by <c> | </c> in report order.
/// </summary>
internal readonly record struct Pin(string Reason, string Problem);
