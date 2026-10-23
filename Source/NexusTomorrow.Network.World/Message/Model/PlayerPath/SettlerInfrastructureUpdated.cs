using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server0674, Direction.ServerToClient)]
public sealed partial class SettlerInfrastructureUpdated
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
