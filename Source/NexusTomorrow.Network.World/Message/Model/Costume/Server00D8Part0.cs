using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireStruct]
public sealed partial class Server00D8Part0
{
    public const byte MaxCostumeItems = 7;

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(2)] public byte Unknown2 { get; set; }
    [FixedArray(7), UInt(32)] public uint[] Unknown3 { get; set; } = new uint[7];
    [FixedArray(7), UInt(32)] public uint[] Unknown4 { get; set; } = new uint[7];
}
