using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ClientRandomRollRequest)]
[WireMessage(GameMessageOpcode.ClientRandomRollRequest, Direction.ClientToServer)]
public sealed partial class ClientRandomRollRequest : IReadable
{
    // Note that for /roll on the command line,
    // the TargetPlayerIdentity's realmId and
    // characterId, as well as Unknown0 are 0.
    // As such, these fields are currently unused.
    public Identity UnusedIdentity { get; } = new();
    [UInt(32)] public uint MinRandom { get; private set; }
    [UInt(32)] public uint MaxRandom { get; private set; }
    [UInt(32)] public uint Unused { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnusedIdentity.Read(reader);
        MinRandom = reader.ReadUInt();
        MaxRandom = reader.ReadUInt();
        Unused = reader.ReadUInt();
    }
}
