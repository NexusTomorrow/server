using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistAddSpecimenSurveyToCreature)]
[WireMessage(GameMessageOpcode.ServerPathScientistAddSpecimenSurveyToCreature, Direction.ServerToClient)]
public sealed partial class ServerPathScientistAddSpecimenSurveyToCreature : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathScientistSpecimenSurveyId { get; set; }
    [UInt(32)] public uint ObjectiveIndex { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(PathScientistSpecimenSurveyId, 14);
        writer.Write(ObjectiveIndex);
    }
}
