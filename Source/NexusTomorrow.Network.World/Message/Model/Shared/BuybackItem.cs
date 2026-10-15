using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class BuybackItem
{
    [UInt(32)] public uint UniqueId { get; set; }
    [UInt(18)] public uint ItemId { get; set; }
    [UInt(32)] public uint Quantity { get; set; }
    [UInt(64)] public ulong Unk3 { get; set; }
    [UInt(32)] public uint Unk4 { get; set; }
    [UInt(64)] public ulong Unk5 { get; set; }
    [UInt(64)] public ulong Unk6 { get; set; }
    [UInt(18)] public uint Unk7 { get; set; }
    [FixedArray(20), UInt(8)] public byte[] Unk8 { get; set; } = new byte[20];
    [FixedArray(32), UInt(8)] public byte[] Unk9 { get; set; } = new byte[32];
    [FixedArray(2), UInt(64)] public ulong[] CurrencyAmount { get; set; } = new ulong[2];
    [FixedArray(2), UInt(4)] public CurrencyType[] CurrencyTypeId { get; set; } = new CurrencyType[2];
    [UInt(32)] public uint UnkE { get; set; }
}
