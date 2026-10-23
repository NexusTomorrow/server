using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// A unit yells a line.
/// </summary>
/// <remarks>
/// The client posts the line on the NPCYell channel with a speech bubble, or under the creature's name when the unit is gone.
/// </remarks>
[WireMessage(GameMessageOpcode.Server075C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitYell
{
    /// <summary>
    /// The line.
    /// </summary>
    public Server0738Part0 Text { get; set; } = new();

    /// <summary>
    /// The unit yelling, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The creature whose name the line goes under when the unit is not there, a Creature2 table id.
    /// </summary>
    [UInt(18)] public uint CreatureId { get; set; }

    /// <summary>
    /// How far the line carries.
    /// </summary>
    [Float32] public float Range { get; set; }
}
