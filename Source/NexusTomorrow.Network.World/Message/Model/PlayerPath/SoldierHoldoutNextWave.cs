using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Starts a soldier event's next wave.
/// </summary>
/// <remarks>
/// The client keeps the wave and the boss flag and restarts the event's phase clock, except for the four timed event types (PathSoldierEvent
/// types 2, 3, 6 and 8), whose phase keeps running. It sets the sound zone kit the wave's PathSoldierEventWave row names and raises
/// SoldierHoldoutNextWave.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0679, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierHoldoutNextWave
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }

    /// <summary>
    /// The wave, by its PathSoldierEventWave row's waveIndex; SoldierEvent:GetWavesReleased counts the event's waves up to it.
    /// </summary>
    [UInt(32)] public uint WaveIndex { get; set; }

    /// <summary>
    /// Whether the wave is a boss wave, which SoldierEvent:IsBoss returns.
    /// </summary>
    [UInt(1)] public bool IsBoss { get; set; }
}
