using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0935Part0
{
    [UInt(5)] public byte Unknown0 { get; set; }
    [Float32] public float Unknown1 { get; set; }
}
