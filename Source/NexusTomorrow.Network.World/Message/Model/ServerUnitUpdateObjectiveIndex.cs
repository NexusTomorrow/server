using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// ObjectiveIndex is used for PathMissions, Quests, Challenges, Datacubes, PublicEvents, Achievements.
/// </summary>
[Message(GameMessageOpcode.ServerUnitUpdateObjectiveIndex)]
[WireMessage(GameMessageOpcode.ServerUnitUpdateObjectiveIndex, Direction.ServerToClient)]
public sealed partial class ServerUnitUpdateObjectiveIndex : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint ObjectiveIndex { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(ObjectiveIndex);
    }
}
