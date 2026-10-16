using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0111Part0
{
    public Server0111Part1 Unknown0 { get; set; }
    [UInt(6)] public byte Unknown1 { get; set; }
}
