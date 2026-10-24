namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Every declared message that disagrees with the client facts, keyed by the opcode, side and path each problem names (the text before its colon,
/// for example <c>0x013C server to client [1].element[1]</c>), each with its reason and the problem text after the colon. The registry check fails
/// on a disagreement not listed here, on a listed one that no longer occurs and on a listed one whose problem text has changed, so the list only
/// shrinks as the reading settles each one.
/// </summary>
internal static class RegistryDisagreements
{
    private const string BitsDiffer = "unsettled: the declaration puts a different number of bits on the wire than the client takes";
    private const string PartMissing = "unsettled: the client takes a part the declaration lacks";
    private const string StringAsCountAndUnits = "unsettled: the bits agree, but the client reads the string as a 16-bit count and that many 16-bit units";
    private const string TreeDiffers = "unsettled: the client's full layout differs; settled when its area is read";
    private const string ReadAsFloat = "unsettled: the client reads the field as a float and the declaration as an integer; its encoding waits on the RE seat's ruling";

    public static IReadOnlyDictionary<string, Pin> Pinned { get; } = new Dictionary<string, Pin>(StringComparer.Ordinal)
    {
        ["0x00FC client to server [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client00FC
        ["0x00FC client to server [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client00FC
        ["0x00FC client to server [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client00FC
        ["0x010F server to client [14]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server010F
        ["0x010F server to client [15]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server010F
        ["0x010F server to client [16]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server010F
        ["0x010F server to client [17]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server010F
        ["0x010F server to client [18]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server010F
        ["0x010F server to client [27]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server010F
        ["0x0112 server to client [5].element[7].arm 0x03"] = new(TreeDiffers, "the declaration's sequence goes on for 1 nodes after the client's ends"), // Server0112
        ["0x0117 server to client [2].element[27]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // CharacterList
        ["0x0132 server to client [7].arm 0x03"] = new(TreeDiffers, "the declaration's sequence goes on for 1 nodes after the client's ends"), // Server0132
        ["0x0134 server to client [7].arm 0x03"] = new(TreeDiffers, "the declaration's sequence goes on for 1 nodes after the client's ends"), // Server0134
        ["0x013C server to client [1].element[1]"] = new(TreeDiffers, "the client has 32, the declaration 3"), // Server013C
        ["0x0152 server to client [1].element[0]"] = new(TreeDiffers, "the client has 8, the declaration 3"), // Server0152
        ["0x01A9 server to client [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server01A9
        ["0x033D server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // SubscriptionState
        ["0x0348 server to client [0]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [10]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [11]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [5]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [6]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [7]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [8]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x0348 server to client [9]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0348
        ["0x034C server to client [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034C
        ["0x034C server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034C
        ["0x034C server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034C
        ["0x034C server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034C
        ["0x034D server to client [0]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [10]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [11]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [12]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [13]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [14]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [15]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [5]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [6]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [7]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [8]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x034D server to client [9]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server034D
        ["0x0351 server to client [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0351 server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0351 server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0351 server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0351 server to client [5]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0351 server to client [6]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0351 server to client [7]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0351
        ["0x0359 server to client [2].element[1]"] = new(TreeDiffers, "the client accepts selector 2 and the declaration has no arm"), // Server0359
        ["0x0359 server to client [2].element[1].arm 0x01[0]"] = new(TreeDiffers, "the client has a narrow string, the declaration a wide string"), // Server0359
        ["0x035F server to client [5].element[5]"] = new(TreeDiffers, "the client has a union, the declaration an array"), // Server035F
        ["0x0459 client to server [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client0459
        ["0x0459 client to server [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client0459
        ["0x0459 client to server [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client0459
        ["0x045A server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server045A
        ["0x045A server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server045A
        ["0x045A server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server045A
        ["0x0598 server to client [3]"] = new(StringAsCountAndUnits, "the client has 16, the declaration a terminated wide string"), // PlayerInfoResponse
        ["0x0599 server to client [3]"] = new(StringAsCountAndUnits, "the client has 16, the declaration a terminated wide string"), // PlayerInfoDetailsResponse
        ["0x05A1 server to client [1].element[2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server05A1
        ["0x0718 server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0718
        ["0x074A server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server074A
        ["0x074A server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server074A
        ["0x076A server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // GuildRecruitmentDetails
        ["0x081A client to server [4].element[0]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[5]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[6]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[7]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A client to server [4].element[8]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[0]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[5]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[6]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[7]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081A server to client [4].element[8]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081A
        ["0x081B client to server [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081B
        ["0x081B client to server [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081B
        ["0x081B client to server [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081B
        ["0x081B server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081B
        ["0x081B server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081B
        ["0x081B server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Message081B
        ["0x0823 client to server [1]"] = new(PartMissing, "the client has 32, the declaration a blob"), // Client0823
        ["0x0828 client to server [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client0828
        ["0x082A client to server [0]"] = new(BitsDiffer, "the client has 32, the declaration 20"), // Client082A
        ["0x082C client to server [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Client082C
        ["0x0846 server to client [0]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0846
        ["0x084B server to client [4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server084B
        ["0x084C server to client [1].element"] = new(TreeDiffers, "the declaration's sequence goes on for 1 nodes after the client's ends"), // Server084C
        ["0x084C server to client [1].element[4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server084C
        ["0x0855 server to client [1]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0855
        ["0x0855 server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0855
        ["0x0905 server to client [6]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // UnitVisualsChanged
        ["0x092C server to client [1].element[3]"] = new(TreeDiffers, "the client has 32, the declaration a union"), // RewardPropertiesChanged
        ["0x093D server to client [2]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // UnitVitalMaxChanged
        ["0x0967 server to client"] = new(BitsDiffer, "the declaration's sequence goes on for 1 nodes after the client's ends"), // Server0967
        ["0x098B server to client [1].element[6].element[11].element[1]"] = new(TreeDiffers, "the client accepts selector 3 and the declaration has no arm"), // Server098B
        ["0x098B server to client [1].element[6].element[11].element[1].arm 0x01"] = new(TreeDiffers, "the client's sequence goes on for 1 nodes after the declaration's ends"), // Server098B
        ["0x098B server to client [1].element[6].element[11].element[1].arm 0x02"] = new(TreeDiffers, "the client's sequence goes on for 1 nodes after the declaration's ends"), // Server098B
        ["0x098E server to client [1].element[4]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server098E
        ["0x098F server to client [3]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server098F
        ["0x0991 server to client [10]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0991
        ["0x0991 server to client [11]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0991
        ["0x0991 server to client [12]"] = new(ReadAsFloat, "the client reads a float, the declaration an integer"), // Server0991
    };
}

/// <summary>
/// One pinned disagreement: why it stands, and the problem text the check reports for it, several joined by <c> | </c> in report order.
/// </summary>
internal readonly record struct Pin(string Reason, string Problem);
