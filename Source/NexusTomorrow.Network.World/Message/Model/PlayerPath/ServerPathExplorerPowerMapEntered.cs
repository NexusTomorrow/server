using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever the player enters an area where a Tracking mission can be started

[WireMessage(GameMessageOpcode.ServerPathExplorerPowerMapEntered, Direction.ServerToClient)]
public sealed partial class ServerPathExplorerPowerMapEntered
{
    [UInt(14)] public ushort PathMissionId { get; set; }
}
