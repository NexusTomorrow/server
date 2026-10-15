using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathScientistAddSpecimenSurveyToCreature, Direction.ServerToClient)]
public sealed partial class ServerPathScientistAddSpecimenSurveyToCreature
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathScientistSpecimenSurveyId { get; set; }
    [UInt(32)] public uint ObjectiveIndex { get; set; }
}
