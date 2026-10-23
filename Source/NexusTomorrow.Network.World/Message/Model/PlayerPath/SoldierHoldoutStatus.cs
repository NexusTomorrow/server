using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Sets a soldier event's state: its units, speaker, wave, boss flag, mode, maximum healths and clocks.
/// </summary>
/// <remarks>
/// The client replaces the event's units with <see cref="Units"/> and clears its built improvements and build credits. When it holds no
/// start for the event it takes the start as <see cref="EventElapsedMs"/> before now, and it sets the phase clock so that
/// SoldierEvent.GetElapsedTime reads <see cref="PhaseElapsedMs"/> on arrival. In <see cref="PlayerPathSoldierEventMode.Active"/> the
/// speaker says a line picked by the event's type. It then sets the sound zone kit the wave's PathSoldierEventWave row names and raises
/// SoldierHoldoutStatus.
/// </remarks>
[WireMessage(GameMessageOpcode.Server067B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierHoldoutStatus
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// The event's units, each with its type.
    /// </summary>
    [Array(CountBits = 32)] public List<SoldierEventUnit> Units { get; set; } = [];

    /// <summary>
    /// The unit that speaks for the event, by unit id.
    /// </summary>
    [UInt(32)] public uint SpeakerUnitId { get; set; }

    /// <summary>
    /// Whether the current wave is a boss wave, which SoldierEvent.IsBoss returns.
    /// </summary>
    [UInt(1)] public bool IsBoss { get; set; }

    /// <summary>
    /// The event's mode, which SoldierEvent.GetState returns. SoldierEvent.GetMaxTime gives the tower defense build time in
    /// <see cref="PlayerPathSoldierEventMode.Setup"/> and the initial spawn time in <see cref="PlayerPathSoldierEventMode.InitialDelay"/>.
    /// </summary>
    [UInt(32)] public PlayerPathSoldierEventMode Mode { get; set; }

    /// <summary>
    /// The time spent in the current phase in milliseconds, which SoldierEvent.GetElapsedTime returns on arrival.
    /// </summary>
    [UInt(32)] public uint PhaseElapsedMs { get; set; }

    /// <summary>
    /// The current wave, by its PathSoldierEventWave row's waveIndex.
    /// </summary>
    [UInt(32)] public uint WaveIndex { get; set; }

    /// <summary>
    /// The maximum health of the event's defend units, which SoldierEvent.GetMaxDefendHealth returns.
    /// </summary>
    [Float32] public float MaxDefendHealth { get; set; }

    /// <summary>
    /// The maximum health of the event's auxiliary units, which SoldierEvent.GetMaxAuxiliaryHealth returns.
    /// </summary>
    [Float32] public float MaxAuxiliaryHealth { get; set; }

    /// <summary>
    /// The time since the event started in milliseconds; the client takes it only when it holds no start for the event.
    /// </summary>
    [UInt(32)] public uint EventElapsedMs { get; set; }
}
