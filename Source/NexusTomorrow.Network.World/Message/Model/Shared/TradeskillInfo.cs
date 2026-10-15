using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class TradeskillInfo : IWritable
{
    [UInt(32)] public TradeskillType TradeskillId { get; set; }
    [UInt(32)] public uint TradeskillXp { get; set; }
    [UInt(32)] public uint IsActive { get; set; }
    [UInt(32)] public uint PropertyProficiencyFlags { get; set; }
    [UInt(32)] public uint TalentPoints { get; set; }
    [FixedArray(10), UInt(32)] public uint[] TradeskillTalentTierIds { get; set; } = new uint[10];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TradeskillId, 32u);
        writer.Write(TradeskillXp);
        writer.Write(IsActive);
        writer.Write(PropertyProficiencyFlags);
        writer.Write(TalentPoints);
        foreach (uint tierId in TradeskillTalentTierIds)
        {
            writer.Write(tierId);
        }
    }
}
