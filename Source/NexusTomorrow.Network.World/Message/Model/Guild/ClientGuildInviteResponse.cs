using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientGuildInviteResponse)]
[WireMessage(GameMessageOpcode.ClientGuildInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientGuildInviteResponse : IReadable
{
    [UInt(1)] public bool Accepted { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Accepted = reader.ReadBit();
    }
}
