using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientGroupRequestJoinResponse)]
[WireMessage(GameMessageOpcode.ClientGroupRequestJoinResponse, Direction.ClientToServer)]
public sealed partial class ClientGroupRequestJoinResponse : IReadable
{
    [UInt(64)] public ulong GroupId { get; set; }

    [UInt(1)] public bool AcceptedRequest { get; set; }

    [WideString] public string InviteeName { get; set; }


    public void Read(GamePacketReader reader)
    {
        GroupId = reader.ReadULong();
        AcceptedRequest = reader.ReadBit();
        InviteeName = reader.ReadWideString();
    }
}
