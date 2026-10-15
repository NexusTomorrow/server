using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupLeave)]
[WireMessage(GameMessageOpcode.ClientGroupLeave, Direction.ClientToServer)]
public sealed partial class ClientGroupLeave : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(1)] public bool ShouldDisband { get; set; }


    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        ShouldDisband = reader.ReadBit();
    }
}
