using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ClientPlayerInfoRequest, Direction.ClientToServer)]
public sealed partial class ClientPlayerInfoRequest
{
    [UInt(4)] public PlayerInfoRequestType Type { get; private set; }
    public Identity Identity { get; } = new();
}
