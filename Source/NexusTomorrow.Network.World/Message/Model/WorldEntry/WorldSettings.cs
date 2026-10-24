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
/// arrival the client sends <see cref="Option.OptionsSync"/> and <c>0x063B</c>. The client needs it to finish entering the world: it sends
/// <see cref="WorldReady"/> only after it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00F1, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class WorldSettings
{
    /// <summary>
    /// The world's difficulty, which GameLib.GetInstanceSettings reports as eWorldDifficulty. Until this message arrives the client holds 2.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte Difficulty { get; set; }

    /// <summary>
    /// The world's prime level, which GameLib.GetWorldPrimeLevel reports.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PrimeLevel { get; set; }

    /// <summary>
    /// Whether the world scales the player's level, which GameLib.GetInstanceSettings reports as bWorldForcesLevelScaling.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool ForcesLevelScaling { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown2Bit1 { get; set; }

    [UInt(1)] public bool Unknown2Bit2 { get; set; }
    [UInt(1)] public bool Unknown2Bit3 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown2Bit4 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown2Bit5 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown2Bit6 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Unknown2Bit7 { get; set; }

    /// <summary>
    /// The period, in milliseconds, of the client's movement timer, from which the client sends the movement of the unit it moves
    /// (<see cref="ClientMovement"/>). The client clamps it to 25 to 1,000.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MovementInterval { get; set; }
}
