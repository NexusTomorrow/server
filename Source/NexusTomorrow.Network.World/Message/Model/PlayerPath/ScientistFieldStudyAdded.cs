using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Files a scientist field study under a unit's creature.
/// </summary>
[WireMessage(GameMessageOpcode.Server08B5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ScientistFieldStudyAdded
{
    /// <summary>
    /// The unit, by unit id; the client files the study under the unit's creature.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The study, a PathScientistFieldStudy table id.
    /// </summary>
    [UInt(14)] public uint FieldStudyId { get; set; }
}
