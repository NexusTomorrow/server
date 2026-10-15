using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSoldierTriggerOrdersTextOnUnit, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierTriggerOrdersTextOnUnit
{
    [UInt(15)] public ushort PathMissionId { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
}
