using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupInviteResponse)]
[WireMessage(GameMessageOpcode.ClientGroupInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientGroupInviteResponse : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(1)] public bool Response { get; set; }
    [UInt(32)] public uint Unk1 { get; set; }

    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        Response = reader.ReadBit();
        Unk1 = reader.ReadUInt();
    }
}
