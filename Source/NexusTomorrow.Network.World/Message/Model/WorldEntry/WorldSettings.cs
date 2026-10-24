using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Pregame;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// The settings of the world the client is entering.
/// </summary>
/// <remarks>
/// Taken in any state, after <see cref="Pregame.EnterWorld"/>, which clears the difficulty and the readiness this message grants. On
/// arrival the client sends <c>0x00D5</c> and <c>0x063B</c>. The client needs it to finish entering the world: it sends
/// <see cref="WorldReady"/> only after it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00F1, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class WorldSettings
{
    /// <summary>
    /// The world's difficulty, as the client's interface shows it. Until this message arrives the client holds 2.
    /// </summary>
    [UInt(2)] public byte Difficulty { get; set; }

    /// <summary>
    /// The world's prime level, as the client's interface shows it.
    /// </summary>
    [UInt(32)] public uint PrimeLevel { get; set; }

    /// <summary>
    /// Flags. Bit 0: the world scales the player's level, as the client's interface shows it. The client gives bits 1 and 4 to 7 no
    /// meaning.
    /// </summary>
    [UInt(8)] public byte Flags { get; set; }

    /// <summary>
    /// The period, in milliseconds, of the client's movement timer, from which the client sends the movement of the unit it moves
    /// (<see cref="ClientMovement"/>). The client clamps it to 25 to 1,000.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MovementInterval { get; set; }
}
