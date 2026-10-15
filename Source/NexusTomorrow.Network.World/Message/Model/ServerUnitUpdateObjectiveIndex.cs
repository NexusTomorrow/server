using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// ObjectiveIndex is used for PathMissions, Quests, Challenges, Datacubes, PublicEvents, Achievements.
/// </summary>
[WireMessage(GameMessageOpcode.ServerUnitUpdateObjectiveIndex, Direction.ServerToClient)]
public sealed partial class ServerUnitUpdateObjectiveIndex
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint ObjectiveIndex { get; set; }
}
