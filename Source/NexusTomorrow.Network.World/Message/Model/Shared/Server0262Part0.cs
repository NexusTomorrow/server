using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part0
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
