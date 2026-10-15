using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SetterHubReward lua event
[WireMessage(GameMessageOpcode.ServerPathSettlerHubReward, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerHubReward
{
    [UInt(14)] public uint PathSettlerHubId { get; set; }
}
