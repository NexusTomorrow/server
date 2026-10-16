using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0112Part3
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(17)] public uint Unknown1 { get; set; }
}
