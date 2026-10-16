using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server012EPart0
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
    [FixedArray(4), UInt(32)] public uint[] Unknown3 { get; set; } = new uint[4];
}
