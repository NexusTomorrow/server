using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0675, Direction.ServerToClient)]
public sealed partial class SettlerInfrastructureStarted
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
