using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using FactionId = NexusTomorrow.Game.Static.Reputation.Faction;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerPlayerCreate)]
[WireMessage(GameMessageOpcode.ServerPlayerCreate, Direction.ServerToClient)]
public sealed partial class ServerPlayerCreate : IWritable
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

    [Array(CountBits = 32)] public List<InventoryItem> Inventory { get; } = [];
    [FixedArray(16), UInt(64)] public ulong[] Money { get; } = new ulong[16];
    [UInt(32)] public uint Xp { get; set; }
    [UInt(32)] public uint RestBonusXp { get; set; }
    [UInt(32)] public ItemProficiency ItemProficiencies { get; set; }
    [UInt(32)] public uint ElderPoints { get; set; }
    [UInt(32)] public uint DailyElderPoints { get; set; }
    [UInt(3)] public byte SpecIndex { get; set; }
    [UInt(16)] public ushort BonusPower { get; set; }
    [UInt(32)] public uint UnknownA0 { get; set; }
    public Faction FactionData { get; set; }
    [Array(CountBits = 32)] public List<Pet> Pets { get; } = [];
    [UInt(32)] public uint InputKeySet { get; set; }
    [UInt(16)] public ushort UnknownBC { get; set; }
    [UInt(32)] public int ActiveCostumeIndex { get; set; }
    [UInt(32)] public uint UnknownC4 { get; set; }
    [UInt(32)] public uint UnknownC8 { get; set; }
    [Array(CountBits = 6), UInt(16)] public List<ushort> KnownDyes { get; } = [];
    [FixedArray(512), UInt(16)] public ushort[] TradeskillMaterials { get; set; } = new ushort[512];
    [Float32] public float GearScore { get; set; }
    [UInt(1)] public bool IsPvpServer { get; set; }
    [UInt(32)] public uint MatchingEligibilityFlagMask { get; set; } // Mask checked against matchingMapPreqrequisite.matchingEligibilityFlagEnum field. Tbl has no entries though.
    [Array(CountBits = 32)] public List<CharacterEntitlement> CharacterEntitlements { set; get; } = [];

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
