using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class TradeskillInfo
{
    [UInt(32)] public TradeskillType TradeskillId { get; set; }
    [UInt(32)] public uint TradeskillXp { get; set; }
    [UInt(32)] public uint IsActive { get; set; }
    [UInt(32)] public uint PropertyProficiencyFlags { get; set; }
    [UInt(32)] public uint TalentPoints { get; set; }
    [FixedArray(10), UInt(32)] public uint[] TradeskillTalentTierIds { get; set; } = new uint[10];
}
