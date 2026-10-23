using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Files a scientist specimen survey under a unit's creature.
/// </summary>
[WireMessage(GameMessageOpcode.Server08B4, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ScientistSpecimenSurveyAdded
{
    /// <summary>
    /// The unit, by unit id; the client files the survey under the unit's creature.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The survey, a PathScientistSpecimenSurvey table id.
    /// </summary>
    [UInt(14)] public uint SpecimenSurveyId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
