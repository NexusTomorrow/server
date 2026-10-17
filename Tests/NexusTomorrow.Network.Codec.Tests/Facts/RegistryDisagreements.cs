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
    private const string ReadAsBytes = "settled: the client reads these 32-bit values as one run of bytes, the same bits on the wire";

    public static IReadOnlyDictionary<string, Pin> Pinned { get; } = new Dictionary<string, Pin>(StringComparer.Ordinal)
    {
        ["0x0092 server to client [4]"] = new(FieldsDiffer, "the client has 8, the declaration 64"), // Server0092
        ["0x00C7 server to client [1].element[21]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Message00C7
        ["0x00D6 server to client [14]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server00D6
        ["0x00D6 server to client [9].element[4]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Server00D6
        ["0x00D8 server to client [3]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server00D8
        ["0x00D9 server to client [1].element[3]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Server00D9
        ["0x0112 server to client [5].element[7].arm 0x03"] = new(TreeDiffers, "the client's sequence has 1 nodes, the declaration's 2"), // Server0112
        ["0x0112 server to client [9]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server0112
        ["0x012E server to client [65].element[3]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Server012E
        ["0x012F server to client [3]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server012F
        ["0x0130 server to client [4].element[4]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Server0130
        ["0x0132 server to client [7].arm 0x03"] = new(TreeDiffers, "the client's sequence has 1 nodes, the declaration's 2"), // Server0132
        ["0x0134 server to client [7].arm 0x03"] = new(TreeDiffers, "the client's sequence has 1 nodes, the declaration's 2"), // Server0134
        ["0x013C server to client [1].element[1]"] = new(TreeDiffers, "the client has 32, the declaration 3"), // Server013C
        ["0x0152 server to client [1].element[0]"] = new(TreeDiffers, "the client has 8, the declaration 3"), // Server0152
        ["0x015A client to server [1]"] = new(BitsDiffer, "the client has 7, the declaration 32"), // Client015A
        ["0x0168 client to server [8]"] = new(FieldsDiffer, "the client has 8, the declaration 64"), // Client0168
        ["0x018E server to client [9]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Message018E
        ["0x01B4 server to client [1]"] = new(TreeDiffers, "the client has 32, the declaration a blob"), // Server01B4
        ["0x01C0 server to client"] = new(BitsDiffer, "the client's sequence has 6 nodes, the declaration's 5"), // Server01C0
        ["0x01C3 client to server [4].element[3].arm 0x01[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01C3
        ["0x01C3 client to server [4].element[3].arm 0x07[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01C3
        ["0x01C8 server to client [12].element[3].arm 0x01[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Server01C8
        ["0x01C8 server to client [12].element[3].arm 0x07[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Server01C8
        ["0x01D4 client to server [4].element[3].arm 0x01[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01D4
        ["0x01D4 client to server [4].element[3].arm 0x07[0]"] = new(TreeDiffers, "the client has 32, the declaration 14"), // Client01D4
        ["0x023F client to server [3]"] = new(FieldsDiffer, "the client has 32, the declaration 1"), // Client023F
        ["0x0240 client to server [0]"] = new(FieldsDiffer, "the client has 32, the declaration 1"), // Client0240
        ["0x0247 server to client [1]"] = new(TreeDiffers, "the client accepts selector 34 and the declaration has no arm"), // Server0247
        ["0x0247 server to client [1].arm 0x05[5]"] = new(TreeDiffers, "the client has 32, the declaration 1"), // Server0247
        ["0x0247 server to client [1].arm 0x0B"] = new(TreeDiffers, "the client's sequence has 7 nodes, the declaration's 3"), // Server0247
        ["0x0247 server to client [1].arm 0x0F"] = new(TreeDiffers, "the client's sequence has 13 nodes, the declaration's 9"), // Server0247
        ["0x0247 server to client [1].arm 0x23"] = new(TreeDiffers, "the client's sequence has 5 nodes, the declaration's 1"), // Server0247
        ["0x0248 client to server [0]"] = new(FieldsDiffer, "the client has 32, the declaration 1"), // Client0248
        ["0x024C server to client [2]"] = new(FieldsDiffer, "the client has 8, the declaration 64"), // Server024C
        ["0x0252 server to client [0]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server0252
        ["0x0255 client to server [4]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Client0255
        ["0x025E server to client [2]"] = new(FieldsDiffer, "the client has 8, the declaration 64"), // Server025E
        ["0x026A server to client [1]"] = new(FieldsDiffer, "the client has 8, the declaration 64"), // Server026A
        ["0x033F client to server [1]"] = new("unsettled: the declaration puts a different number of bits on the wire than the client takes, and lacks the client's wide string", "the client has 32, the declaration a union"), // Client033F
        ["0x0359 server to client [2].element[1]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server0359
        ["0x0359 server to client [2].element[1].arm 0x01[0]"] = new(TreeDiffers, "the client has a narrow string, the declaration a wide string"), // Server0359
        ["0x035F server to client [5].element[5]"] = new(TreeDiffers, "the client has a union, the declaration an array"), // Server035F
        ["0x0399 server to client [3].element[3]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Server0399
        ["0x03A3 server to client [1].element[8].element[3]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Server03A3
        ["0x03BA server to client [1].element[7]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Server03BA
        ["0x03D1 server to client [4]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server03D1
        ["0x0405 server to client [7]"] = new(FieldsDiffer, "the client has 14, the declaration 8"), // Server0405
        ["0x041F server to client [4].element[2]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Server041F
        ["0x0424 server to client [6]"] = new(FieldsDiffer, "the client has 14, the declaration 8"), // Server0424
        ["0x0427 server to client [10].element[5]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Server0427
        ["0x046F server to client"] = new(BitsDiffer, "the client's sequence has 32 nodes, the declaration's 33"), // Server046F
        ["0x047A server to client [4].element"] = new(TreeDiffers, "the client's sequence has 30 nodes, the declaration's 31"), // Server047A
        ["0x0487 server to client [4]"] = new(FieldsDiffer, "the client has 8, the declaration 64"), // Server0487
        ["0x0489 server to client [4].element[0]"] = new(TreeDiffers, "the client has 64, the declaration 14"), // Server0489
        ["0x0493 server to client [116]"] = new(FieldsDiffer, "the client has 32, the declaration 8"), // Server0493
        ["0x0497 server to client [3].element[5]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Server0497
        ["0x0497 server to client [4].element[1]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Server0497
        ["0x04A0 server to client [3].element[5]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Server04A0
        ["0x04A1 server to client [3]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server04A1
        ["0x04A3 server to client [7]"] = new(FieldsDiffer, "the client has 32, the declaration 8"), // Server04A3
        ["0x04DF server to client [1].element[20]"] = new(TreeDiffers, "the client has 8, the declaration 64"), // Server04DF
        ["0x04E1 server to client [3].element[4]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Server04E1
        ["0x04F5 client to server [1].element[4]"] = new(TreeDiffers, "the client has 32, the declaration 16"), // Client04F5
        ["0x050B client to server [2].element[4]"] = new(TreeDiffers, "the client has 32, the declaration 16"), // Client050B
        ["0x050D client to server [0]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Client050D
        ["0x0510 client to server [7]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Client0510
        ["0x0598 server to client [3]"] = new(TreeDiffers, "the client has 16, the declaration a terminated wide string"), // Server0598
        ["0x0599 server to client [3]"] = new(FieldsDiffer, "the client has 16, the declaration a terminated wide string"), // Server0599
        ["0x05A3 server to client [2].element[16].element[7]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Message05A3
        ["0x05D9 server to client [0]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server05D9
        ["0x05E3 server to client [3]"] = new(FieldsDiffer, "the client has 1, the declaration 8"), // Server05E3
        ["0x05E5 server to client [0]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server05E5
        ["0x05E7 server to client [1]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server05E7
        ["0x05E7 server to client [3]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server05E7
        ["0x05E9 server to client [2]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server05E9
        ["0x05EE server to client [2]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server05EE
        ["0x060A server to client [2].element[6]"] = new(TreeDiffers, "the client has 2, the declaration 14"), // Server060A
        ["0x0667 server to client [1]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server0667
        ["0x066A client to server [0]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Client066A
        ["0x06BC server to client [1]"] = new(ReadAsBytes, "the client has 8, the declaration 32"), // PathState
        ["0x0738 server to client [3].element[0]"] = new(TreeDiffers, "the client has 3, the declaration 32"), // Server0738
        ["0x07CD server to client [2]"] = new(FieldsDiffer, "the client has 32, the declaration 8"), // Server07CD
        ["0x07D3 server to client [1].element[2]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Server07D3
        ["0x07D5 server to client [2]"] = new(FieldsDiffer, "the client has 32, the declaration 8"), // Server07D5
        ["0x07DC client to server [6].element[1].arm 0x04[1]"] = new(TreeDiffers, "the client has 32, the declaration 8"), // Client07DC
        ["0x07DC client to server [6].element[1].arm 0x06[0]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Client07DC
        ["0x0823 client to server [1]"] = new(PartMissing, "the client has 32, the declaration a blob"), // Client0823
        ["0x082A client to server [0]"] = new(BitsDiffer, "the client has 32, the declaration 20"), // Client082A
        ["0x084C server to client [1].element"] = new(TreeDiffers, "the client's sequence has 6 nodes, the declaration's 7"), // Server084C
        ["0x084F client to server [3]"] = new(FieldsDiffer, "the client has 64, the declaration 8"), // Client084F
        ["0x0854 server to client [1]"] = new(FieldsDiffer, "the client has 64, the declaration 8"), // Server0854
        ["0x0856 server to client [1].element[5]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Server0856
        ["0x0860 server to client [5]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server0860
        ["0x08B7 server to client [1]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server08B7
        ["0x0909 server to client [1]"] = new(FieldsDiffer, "the client has 8, the declaration 32"), // Server0909
        ["0x092C server to client [1].element[3]"] = new(TreeDiffers, "the client has 32, the declaration a union"), // Server092C
        ["0x0954 server to client"] = new(BitsDiffer, "the client's sequence has 8 nodes, the declaration's 4"), // Server0954
        ["0x0959 client to server [1].element[1].arm 0x01[0]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Client0959
        ["0x0959 client to server [1].element[1].arm 0x03[0]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Client0959
        ["0x0959 client to server [1].element[1].arm 0x07[1]"] = new(TreeDiffers, "the client has 14, the declaration 8"), // Client0959
        ["0x0967 server to client"] = new(BitsDiffer, "the client's sequence has 4 nodes, the declaration's 5"), // Server0967
        ["0x0978 server to client [1].element[1]"] = new(TreeDiffers, "the client has 1, the declaration 32"), // Server0978
        ["0x098B server to client [1].element[6].element[3]"] = new(TreeDiffers, "the client has 8, the declaration 32"), // Server098B
    };
}

/// <summary>
/// One pinned disagreement: why it stands, and the problem text the check reports for it, several joined by <c> | </c> in report order.
/// </summary>
internal readonly record struct Pin(string Reason, string Problem);
