using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistAddSpecimenSurveyToCreature)]
public class ServerPathScientistAddSpecimenSurveyToCreature : IWritable
{
    public uint UnitId { get; set; }
    public uint PathScientistSpecimenSurveyId { get; set; }
    public uint ObjectiveIndex { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(PathScientistSpecimenSurveyId, 14);
        writer.Write(ObjectiveIndex);
    }
}
