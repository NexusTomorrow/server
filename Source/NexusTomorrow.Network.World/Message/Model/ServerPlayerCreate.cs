using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using FactionId = NexusTomorrow.Game.Static.Reputation.Faction;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPlayerCreate)]
public partial class ServerPlayerCreate : IWritable
{
    [WireStruct]
    public sealed partial class Faction : IWritable
    {
        [WireStruct]
        public sealed partial class FactionReputation : IWritable
        {
            [UInt(14)] public FactionId FactionId { get; set; }
            [Float32] public float Value { get; set; }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(FactionId, 14u);
                writer.Write(Value);
            }
        }

        [UInt(14)] public FactionId FactionId { get; set; }
        [Array(CountBits = 16)] public List<FactionReputation> FactionReputations { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            writer.Write(FactionId, 14u);

            writer.Write((ushort)FactionReputations.Count);
            FactionReputations.ForEach(f => f.Write(writer));
        }
    }

    [WireStruct]
    public sealed partial class Pet : IWritable
    {
        [UInt(32)] public uint Guid { get; set; }
        [UInt(18)] public uint SummoningSpell { get; set; }
        [UInt(5)] public byte ValidStances { get; set; }
        [UInt(5)] public byte Stance { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Guid);
            writer.Write(SummoningSpell, 18u);
            writer.Write(ValidStances, 5u);
            writer.Write(Stance, 5u);
        }
    }

    [WireStruct]
    public sealed partial class CharacterEntitlement : IWritable
    {
        [UInt(14)] public EntitlementType Entitlement { get; set; }
        [UInt(32)] public uint Count { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Entitlement, 14u);
            writer.Write(Count);
        }
    }

    public List<InventoryItem> Inventory { get; } = [];
    public ulong[] Money { get; } = new ulong[16];
    public uint Xp { get; set; }
    public uint RestBonusXp { get; set; }
    public ItemProficiency ItemProficiencies { get; set; }
    public uint ElderPoints { get; set; }
    public uint DailyElderPoints { get; set; }
    public byte SpecIndex { get; set; }
    public ushort BonusPower { get; set; }
    public uint UnknownA0 { get; set; }
    public Faction FactionData { get; set; }
    public List<Pet> Pets { get; } = [];
    public uint InputKeySet { get; set; }
    public ushort UnknownBC { get; set; }
    public int ActiveCostumeIndex { get; set; }
    public uint UnknownC4 { get; set; }
    public uint UnknownC8 { get; set; }
    public List<ushort> KnownDyes { get; } = [];
    public ushort[] TradeskillMaterials { get; set; } = new ushort[512];
    public float GearScore { get; set; }
    public bool IsPvpServer { get; set; }
    public uint MatchingEligibilityFlagMask { get; set; } // Mask checked against matchingMapPreqrequisite.matchingEligibilityFlagEnum field. Tbl has no entries though.
    public List<CharacterEntitlement> CharacterEntitlements { set; get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Inventory.Count);
        Inventory.ForEach(i => i.Write(writer));

        for (uint i = 0u; i < Money.Length; i++)
            writer.Write(Money[i]);

        writer.Write(Xp);
        writer.Write(RestBonusXp);
        writer.Write(ItemProficiencies, 32u);
        writer.Write(ElderPoints);
        writer.Write(DailyElderPoints);
        writer.Write(SpecIndex, 3u);
        writer.Write(BonusPower);
        writer.Write(UnknownA0);

        FactionData.Write(writer);

        writer.Write(Pets.Count);
        Pets.ForEach(p => p.Write(writer));

        writer.Write(InputKeySet);
        writer.Write(UnknownBC);
        writer.Write(ActiveCostumeIndex);
        writer.Write(UnknownC4);
        writer.Write(UnknownC8);

        writer.Write((byte)KnownDyes.Count, 6u);
        KnownDyes.ForEach(a => writer.Write(a));

        for (uint i = 0u; i < TradeskillMaterials.Length; i++)
            writer.Write(TradeskillMaterials[i]);

        writer.Write(GearScore);
        writer.Write(IsPvpServer);
        writer.Write(MatchingEligibilityFlagMask);

        writer.Write(CharacterEntitlements.Count);
        CharacterEntitlements.ForEach(u => u.Write(writer));
    }
}
