using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathInitialize, Direction.ServerToClient)]
public sealed partial class ServerPathInitialise
{
    [UInt(3)] public Path ActivePath { get; set; }
    [FixedArray(4), UInt(32)] public uint[] PathProgress { get; set; } = new uint[4];
    [UInt(4)] public PathUnlockedMask PathUnlockedMask { get; set; }
    [Float32] public float TimeSinceLastActivateInDays { get; set; }
}
