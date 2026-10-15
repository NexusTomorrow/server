using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Fires whenever progress is made on a multi-step path mission.
[WireMessage(GameMessageOpcode.ServerPathMissionAdvanced, Direction.ServerToClient)]
public sealed partial class ServerPathMissionAdvanced
{
    [UInt(14)] public ushort PathMissionId { get; set; }
}
