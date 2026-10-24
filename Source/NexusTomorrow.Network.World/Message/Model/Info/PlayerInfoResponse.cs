using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server0598, Direction.ServerToClient)]
public sealed partial class PlayerInfoResponse
{
    public PlayerInfoSummary Unknown0 { get; set; } = new();
}
