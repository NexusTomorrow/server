using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using FactionId = NexusTomorrow.Game.Static.Reputation.Faction;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerPlayerCreate, Direction.ServerToClient)]
public sealed partial class ServerPlayerCreate
{
    [WireStruct]
    public sealed partial class Faction
    {
        [WireStruct]
        public sealed partial class FactionReputation
        {
            [UInt(14)] public FactionId FactionId { get; set; }
            [Float32] public float Value { get; set; }
        }

        [UInt(14)] public FactionId FactionId { get; set; }
        [Array(CountBits = 16)] public List<FactionReputation> FactionReputations { get; set; } = [];
    }

    [WireStruct]
    public sealed partial class Pet
    {
        [UInt(32)] public uint Guid { get; set; }
        [UInt(18)] public uint SummoningSpell { get; set; }
        [UInt(5)] public byte ValidStances { get; set; }
        [UInt(5)] public byte Stance { get; set; }
    }

    [WireStruct]
    public sealed partial class CharacterEntitlement
    {
        [UInt(14)] public EntitlementType Entitlement { get; set; }
        [UInt(32)] public uint Count { get; set; }
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
}
