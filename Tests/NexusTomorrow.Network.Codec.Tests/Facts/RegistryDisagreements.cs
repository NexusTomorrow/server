namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Every declared message that disagrees with the client facts, keyed by the opcode and side each problem names (the text before its colon), each with
/// its reason. The registry check fails on a disagreement not listed here and on a listed one that no longer occurs, so the list only shrinks as stage 3
/// settles each one.
/// </summary>
internal static class RegistryDisagreements
{
    private const string BitsDiffer = "unsettled: the declaration puts a different number of bits on the wire than the client takes";
    private const string FieldsDiffer = "unsettled: the bits agree, but the client takes them as fields of other widths";
    private const string PartMissing = "unsettled: the client takes a part the declaration lacks";
    private const string NoArmZero = "unsettled: no union arm answers selector 0, so the empty path is incomplete";

    public static IReadOnlyDictionary<string, string> Pinned { get; } = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["0x0092 server to client"] = FieldsDiffer, // Server0092
        ["0x00D6 server to client"] = FieldsDiffer, // Server00D6
        ["0x00D8 server to client"] = FieldsDiffer, // Server00D8
        ["0x0112 server to client"] = FieldsDiffer, // Server0112
        ["0x012F server to client"] = FieldsDiffer, // Server012F
        ["0x0144 client to server"] = PartMissing, // Client0144
        ["0x015A client to server"] = BitsDiffer, // Client015A
        ["0x0168 client to server"] = FieldsDiffer, // Client0168
        ["0x018E server to client"] = FieldsDiffer, // Message018E
        ["0x01C0 server to client"] = BitsDiffer, // Server01C0
        ["0x023F client to server"] = FieldsDiffer, // Client023F
        ["0x0240 client to server"] = FieldsDiffer, // Client0240
        ["0x0248 client to server"] = FieldsDiffer, // Client0248
        ["0x024C server to client"] = FieldsDiffer, // Server024C
        ["0x0252 server to client"] = FieldsDiffer, // Server0252
        ["0x0255 client to server"] = FieldsDiffer, // Client0255
        ["0x025B client to server"] = PartMissing, // Client025B
        ["0x025E server to client"] = FieldsDiffer, // Server025E
        ["0x026A server to client"] = FieldsDiffer, // Server026A
        ["0x033F"] = NoArmZero, // Client033F
        ["0x033F client to server"] = "unsettled: the declaration puts a different number of bits on the wire than the client takes, and lacks the client's wide string", // Client033F
        ["0x03D1 server to client"] = FieldsDiffer, // Server03D1
        ["0x03DB server to client"] = FieldsDiffer, // Server03DB
        ["0x0405 server to client"] = FieldsDiffer, // Server0405
        ["0x0424 server to client"] = FieldsDiffer, // Server0424
        ["0x046F server to client"] = BitsDiffer, // Server046F
        ["0x0487 server to client"] = FieldsDiffer, // Server0487
        ["0x0493 server to client"] = FieldsDiffer, // Server0493
        ["0x04A1 server to client"] = FieldsDiffer, // Server04A1
        ["0x04A3 server to client"] = FieldsDiffer, // Server04A3
        ["0x050B client to server"] = PartMissing, // Client050B
        ["0x050D client to server"] = FieldsDiffer, // Client050D
        ["0x0510 client to server"] = FieldsDiffer, // Client0510
        ["0x058F client to server"] = FieldsDiffer, // Client058F
        ["0x0599 server to client"] = FieldsDiffer, // Server0599
        ["0x05D9 server to client"] = FieldsDiffer, // Server05D9
        ["0x05E3 server to client"] = FieldsDiffer, // Server05E3
        ["0x05E5 server to client"] = FieldsDiffer, // Server05E5
        ["0x05E9 server to client"] = FieldsDiffer, // Server05E9
        ["0x05EE server to client"] = FieldsDiffer, // Server05EE
        ["0x0667 server to client"] = FieldsDiffer, // Server0667
        ["0x066A client to server"] = FieldsDiffer, // Client066A
        ["0x06BC server to client"] = FieldsDiffer, // Server06BC
        ["0x07A1 server to client"] = FieldsDiffer, // Server07A1
        ["0x07CD server to client"] = FieldsDiffer, // Server07CD
        ["0x07D5 server to client"] = FieldsDiffer, // Server07D5
        ["0x0823 client to server"] = PartMissing, // Client0823
        ["0x082A client to server"] = BitsDiffer, // Client082A
        ["0x084F client to server"] = FieldsDiffer, // Client084F
        ["0x0854 server to client"] = FieldsDiffer, // Server0854
        ["0x0860 server to client"] = FieldsDiffer, // Server0860
        ["0x08B7 server to client"] = FieldsDiffer, // Server08B7
        ["0x0909 server to client"] = FieldsDiffer, // Server0909
        ["0x0954 server to client"] = BitsDiffer, // Server0954
        ["0x0967 server to client"] = BitsDiffer, // Server0967
    };
}
