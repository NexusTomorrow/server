using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Names the player's own unit: the client takes it as the player and as the unit it moves.
/// </summary>
/// <remarks>
/// Taken after <see cref="Pregame.EnterWorld"/>. The client refuses it while it already has a player, when no unit has the id (the player's
/// <see cref="UnitCreate"/> comes first), and when that unit has no faction. The client needs it to finish entering the world; until
/// it arrives the client ignores <see cref="MovementControl"/> and <see cref="PlayerPath.PathState"/>, and refuses the player's own state
/// (<c>0x025E</c>).
/// </remarks>
[WireMessage(GameMessageOpcode.Server019B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PlayerUnit
{
    /// <summary>
    /// The player's unit, by the id its <see cref="UnitCreate"/> gave it.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }
}
