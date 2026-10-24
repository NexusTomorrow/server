using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server0855, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server0855
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
