using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientGuildInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientGuildInviteResponse
{
    [UInt(1)] public bool Accepted { get; private set; }
}
