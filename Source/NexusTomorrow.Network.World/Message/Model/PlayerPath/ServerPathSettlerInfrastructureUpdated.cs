using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ServerPathSettlerInfrastructureUpdated, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerInfrastructureUpdated
{
    [UInt(32)] public uint PathMissionId { get; set; }
    [UInt(2)] public SettlerInfrastructureState State { get; set; }
    [Float32] public float PercentComplete { get; set; }
}
