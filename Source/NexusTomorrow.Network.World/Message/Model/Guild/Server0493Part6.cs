using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class Server0493Part6
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [FixedArray(10), UInt(32)] public uint[] Unknown1 { get; set; } = new uint[10];
    [UInt(64)] public ulong Unknown2 { get; set; }
}
