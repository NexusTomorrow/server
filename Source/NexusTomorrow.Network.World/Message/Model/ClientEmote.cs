using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientEmote)]
[WireMessage(GameMessageOpcode.ClientEmote, Direction.ClientToServer)]
public sealed partial class ClientEmote : IReadable
{
    [UInt(14)] public ushort EmoteId { get; private set; }
    [UInt(32)] public uint Seed { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(1)] public bool Targeted { get; set; }
    [UInt(1)] public bool Silent { get; set; }

    public void Read(GamePacketReader reader)
    {
        EmoteId = reader.ReadUShort(14u);
        Seed = reader.ReadUInt();
        TargetUnitId = reader.ReadUInt();
        Targeted = reader.ReadBit();
        Silent = reader.ReadBit(); // Seems to be true when the Client is instructed by the server to do this
    }
}
