using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0490, Direction.ClientToServer)]
public sealed partial class GuildInviteAnswer
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
