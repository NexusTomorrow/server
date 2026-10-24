using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// The player's own state: the amount of each currency, experience, item proficiencies, entitlements and other values the client keeps
/// for the player.
/// </summary>
/// <remarks>
/// Taken only after <see cref="PlayerUnit"/>; before it the client refuses it and keeps none of it. The client sends nothing in reply, and
/// does not need it to finish entering the world.
/// </remarks>
[WireMessage(GameMessageOpcode.Server025E, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PlayerState
{
    [WireStruct]
    public sealed partial class PlayerReputations
    {
        [WireStruct]
        public sealed partial class FactionReputation
        {
            /// <summary>
            /// The faction, as an id in the client's Faction2 table.
            /// </summary>
            [VerifiedMember, UInt(14)] public ushort FactionId { get; set; }

            /// <summary>
            /// The player's reputation with the faction. GameLib.GetReputationInfo reports it as nCurrent, with the reputation level it
            /// reaches as nLevel.
            /// </summary>
            [VerifiedMember, Float32] public float Value { get; set; }
        }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [VerifiedMember, UInt(14)] public ushort Unknown0 { get; set; }

        /// <summary>
        /// The factions, each with the player's reputation.
        /// </summary>
        [VerifiedMember, Array(CountBits = 16)] public List<FactionReputation> Factions { get; set; } = [];
    }

    [WireStruct]
    public sealed partial class CharacterEntitlement
    {
        /// <summary>
        /// The entitlement's id, which AccountItemLib.GetCharacterEntitlements reports as nId.
        /// </summary>
        [VerifiedMember, UInt(14)] public ushort EntitlementId { get; set; }

        /// <summary>
        /// The entitlement's count, which AccountItemLib.GetCharacterEntitlements reports as nCount. The client caps it at the
        /// maxCount of the entitlement's Entitlement table row.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Count { get; set; }
    }

    [Array(CountBits = 32)] public List<Server0111Part0> Unknown0 { get; } = [];

    /// <summary>
    /// The amount of each currency the player holds: entry n is the amount of currency type n + 1. The client reads entries 0 to 13 only
    /// (types 1 to 14): GameLib.GetPlayerCurrency reports them, and the client tests costs against them.
    /// </summary>
    [VerifiedMember, FixedArray(15), UInt(64)] public ulong[] Currencies { get; } = new ulong[15];

    /// <summary>
    /// A limit on the credits the player may move, which GameLib.GetMoneyTradeLimit reports. The limit applies only while the client
    /// holds an entry from <see cref="Reward.RewardPropertiesChanged"/> for reward property 20 of kind 1 with a non-zero value; with no
    /// such entry, or with a kind 0 entry or a value of 0, the getter answers nothing. While it applies, the client refuses credits above
    /// it in a trade or a guild bank deposit.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong MoneyTradeLimit { get; set; }

    /// <summary>
    /// The player's total experience. The client works out from it which level the total falls in.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Experience { get; set; }

    /// <summary>
    /// The player's rest experience, as the client's interface shows it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint RestExperience { get; set; }

    /// <summary>
    /// The player's item proficiencies: a mask the client tests against each entry's bit in its ItemProficiency table, for whether the
    /// player can equip an item and may roll need on it.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ItemProficiencies { get; set; }

    /// <summary>
    /// The player's elder points, as the client's interface shows them.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ElderPoints { get; set; }

    /// <summary>
    /// The player's periodic elder points, as the client's interface shows them.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PeriodicElderPoints { get; set; }

    /// <summary>
    /// The index of the player's active ability set, counted from 0. The client's interface reports it counted from 1.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte ActiveActionSet { get; set; }

    /// <summary>
    /// A signed amount the client adds to the player's augmentation point total, which it otherwise works out from the player's level.
    /// </summary>
    [VerifiedMember, SInt(16)] public short AugmentationPointBonus { get; set; }

    [UInt(32)] public uint Unknown9 { get; set; }

    /// <summary>
    /// The player's reputation with each faction listed.
    /// </summary>
    [VerifiedMember] public PlayerReputations Reputations { get; set; } = new();

    /// <summary>
    /// The pets the player has summoned, which the client keeps as it keeps the one <see cref="Pet.PetSpawned"/> adds.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<SummonedPet> Pets { get; } = [];

    /// <summary>
    /// Which of the player's key binding sets is in use, as the client's interface shows it: 3 or 4. The client takes any other value as 3.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint InputKeySet { get; set; }

    /// <summary>
    /// The player's bind point, as an id in the client's BindPoint table, which GameLib.HasBindPoint looks up.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort BindPointId { get; set; }

    /// <summary>
    /// The index of the costume the player wears, counted from 0. The client's interface reports it plus one, so 0xFFFFFFFF reports as 0.
    /// Until a value arrives the client holds 0xFFFFFFFF.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint CostumeIndex { get; set; }

    /// <summary>
    /// The attribute points the player has to spend, which GameLib.GetAttributePoints reports.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint AttributePoints { get; set; }

    /// <summary>
    /// The cooldown on the player's PvP flag, in milliseconds from when the client takes this message. GameLib.GetPvpFlagInfo reports
    /// what remains of it as nCooldown.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PvpFlagCooldown { get; set; }

    /// <summary>
    /// The augmentations the client reports as cached, by id: AbilityBook.GetEldanAugmentationData sets bIsCached on each augmentation
    /// whose id is listed.
    /// </summary>
    [VerifiedMember, Array(CountBits = 6), UInt(16)] public List<ushort> CachedAugments { get; } = [];

    /// <summary>
    /// The count of each item kind in the player's supply satchel, one entry for each satchel index from 0 to 511, as the client's
    /// interface shows them; an item's satchel index picks its entry. The client takes only the entries that are not 0: an entry of 0
    /// leaves the client's count as it was.
    /// </summary>
    [VerifiedMember, FixedArray(512), UInt(16)] public ushort[] SupplySatchel { get; set; } = new ushort[512];

    /// <summary>
    /// The player's gear score, which GameLib.GetGearScore reports. <see cref="GearScoreChanged"/> sets it too.
    /// </summary>
    [VerifiedMember, Float32] public float GearScore { get; set; }

    /// <summary>
    /// Whether the server is a PvP server, as the client's interface shows it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool IsPvpServer { get; set; }

    /// <summary>
    /// The player's matching privileges, a mask, as <see cref="MatchingGame.MatchingEligibilityChanged.Privileges"/> sets them: the client
    /// tests each map's required privileges against it before it offers or queues the map.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint MatchingPrivileges { get; set; }

    /// <summary>
    /// The character's entitlements, each an id and a count, as the client's interface shows them.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<CharacterEntitlement> Entitlements { set; get; } = [];
}
