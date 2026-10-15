using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

//Fires when the unit that is being tracked in a Tracking mission reaches its destination before the player.

[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapWaiting, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapWaiting
{
    [UInt(14)] public ushort PathMissionId { get; set; }
}
