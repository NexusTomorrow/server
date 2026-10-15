using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientStatisticsWatchdog, Direction.ClientToServer)]
public sealed partial class ClientStatisticsWatchdog
{
    [UInt(64)] public ulong PropertyHashSeed { get; private set; }
    [UInt(64)] public ulong PlayerUnitPropertiesHash { get; private set; } // Client iterates over the entire Property array of the player's unit to calculate this hash.
    [UInt(32)] public int LongestTimeBetweenWatchdogLoops { get; private set; } // An average of the last 30 watchdog loop times. Is approximately 30s when performance is good.
    [Float32] public float TimeToMiddleOfCircularBuffer { get; private set; } // Should be not much higher than 1000ms when good.
    [Float32] public float TimeBetweenWatchdogRunsWeightedAverage { get; private set; }
    [Float32] public float WatchdogWeightedAverageError { get; private set; }
    [UInt(32)] public uint UnknownPlayerPositionRelated { get; private set; }
}
