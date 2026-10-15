extern alias legacy;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Game.Static.Entity;
using LegacyModel = legacy::NexusTomorrow.Network.World.Message.Model;
using Model = NexusTomorrow.Network.World.Message.Model;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// The production pairing: every converted model in Network, Network.Auth and Network.World, with its legacy twin of the same full name in the legacy copy,
/// and the pinned rules for the legacy shapes member names cannot follow.
/// </summary>
internal static class ConvertedModels
{
    private const string ReadShortThrows = "the legacy Read takes its first field with ReadShort(32), which throws, so the legacy model never reads; stage 3 declares it from the client facts";
    private const string ChatSeedWidths = "the legacy Write puts the random text seed in 32 bits where the legacy Read takes 14; the declaration follows the Read";
    private const string NoCount = "the legacy Write leaves out the 32-bit count the client facts put before the elements; the declaration writes it";
    private const string NestsItem = "it nests Item, whose legacy Write sends Glyphs.Count as the count of Unknown88";
    private const string FloatHeldInteger = "the legacy model keeps this integer in a float, which holds every integer below 2^24 exactly";
    private const string ByteEnumIn14Bits = "the legacy Read takes 14 bits into a byte-backed enum, and Enum.ToObject keeps the low 8, so these bits change nothing";
    private const string SelectorSwitch = "the legacy Write switches on the selector to pick the fields it writes, and each arm's fields differ in width or name, so a wrong case value writes another arm's fields and fails the write check";
    private const string ModifierTypeSwitch = "the legacy Write switches on ModifierType between a float and a uint cast, so a wrong Discrete value fails the write check; AdditiveScalar and MultiplicativeScalar write the same float, so their values are the RewardModifierValueType members of the arms' names";
    private const string InitialStatList = "the legacy Write replaces every element of SomeStatList with a new UnknownStruct0 before writing it, so only its initial values reach the wire";
    private const string NestsLockedReward = "it nests LockedReward, which follows the client where the seed does not";
    private const uint FloatExactIntegers = (1u << 24) - 1;

    private static readonly Assembly s_legacyAssembly = typeof(legacy::NexusTomorrow.Network.World.ServiceCollectionExtensions).Assembly;

    private static readonly Assembly[] s_declaringAssemblies =
    [
        typeof(NexusTomorrow.Network.WireMessages).Assembly,
        typeof(NexusTomorrow.Network.Auth.WireMessages).Assembly,
        typeof(NexusTomorrow.Network.World.WireMessages).Assembly,
    ];

    /// <summary>
    /// Declared types the check skips, by full name, each with its reason. Every skip is a line here, so none passes unreviewed.
    /// </summary>
    public static IReadOnlyDictionary<string, string> Exclusions { get; } = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["NexusTomorrow.Network.World.Entity.Command.SetPositionMultiSplineCommand"] = "the legacy Read takes Speed as a 16-bit packed float and Position and both location heights through ReadUInt, a numeric conversion; the Write puts Speed as a 32-bit float and all four as IEEE bits; the declaration takes the Read's widths as floats",
        ["NexusTomorrow.Network.World.Entity.Command.SetPositionProjectileCommand"] = "the legacy Read takes the float Gravity through ReadUInt, a numeric conversion, where the Write puts its IEEE bits; the declaration reads it as a float",
        ["NexusTomorrow.Network.World.Entity.Command.SetRotationMultiSplineCommand"] = "the legacy Read takes the floats Position and both location heights through ReadUInt, a numeric conversion, where the Write puts their IEEE bits; the declaration reads them as floats",
        ["NexusTomorrow.Network.World.Entity.Command.SetRotationSplineCommand"] = "the legacy Read takes the float Position through ReadUInt, a numeric conversion, where the Write puts its IEEE bits; the declaration reads it as a float",
        ["NexusTomorrow.Network.World.Message.Model.AccountInventory.ServerAccountOperationResult"] = "the client takes the operation and the result in 32 bits each where the seed wrote 64 each; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Achievement.ClientSteamAchievements"] = "the legacy Read decodes the achievement bytes into an ASCII string, which no declaration holds; the declaration keeps the bytes",
        ["NexusTomorrow.Network.World.Message.Model.Cinematic.ClientCinematicCameraSubjectPosVel"] = "the legacy Read calls Read on a Position it never creates, so it always throws; the declaration creates it",
        ["NexusTomorrow.Network.World.Message.Model.ClientMatchingRoleCheckResponse"] = "the client sends the roles in 32 bits where the seed read 64; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Crafting.ServerCraftingCurrentCraft"] = "it nests CraftStats, whose legacy Write packs its bytes high byte first where its Read unpacks them low byte first",
        ["NexusTomorrow.Network.World.Message.Model.Crafting.ServerTradeskillSigilResult"] = "the client takes the sigil result in 4 bits where the seed wrote 32; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Guild.ClientGuildBankTransaction2"] = "the client sends the source item id in 18 bits where the seed read 32; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildBankInventoryAdd"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildBankTabInventory"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildClassification"] = "the client takes the classification in 3 bits where the seed wrote 14; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerUnitMemberOfGuildChange"] = "the client takes the guild id count in 8 bits where the seed wrote 32; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Housing.ClientHousingChangeInteriorWallpaper"] = "the legacy Read calls Read on the six null elements of DecorUpdates, so it always throws; the declaration follows the client facts",
        ["NexusTomorrow.Network.World.Message.Model.Housing.Neighbourhood"] = "the client takes the neighbourhood info id in 14 bits where the seed wrote 64; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Housing.ServerHousingAttachUnitToDecorItem"] = "the client takes the attached unit id in 32 bits where the seed wrote 64; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Housing.ServerHousingDefaultResidenceNameUpdate"] = "it derives from Neighbourhood, which follows the client where the seed does not",
        ["NexusTomorrow.Network.World.Message.Model.Housing.ServerHousingNeighbourUpdate"] = "the client takes the result in 3 bits where the seed wrote 7; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Housing.ServerHousingNeighbourhoods"] = "it nests Neighbourhood, which follows the client where the seed does not",
        ["NexusTomorrow.Network.World.Message.Model.Info.ServerPrerequisiteFailure"] = "the client takes the localized string id in 21 bits where the seed wrote 15; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Info.ServerRealmInfoResponse"] = "the client takes the realm id in 14 bits where the seed wrote 32; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Map.ServerZoneMap"] = "the legacy Write sends Count / 8 as the byte length, from a Count the caller sets apart from the bytes; the declaration sends the bytes' own length",
        ["NexusTomorrow.Network.World.Message.Model.Pet.ClientPetSetStance"] = "the client sends the stance in 5 bits where the seed read 8; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.PlayerPath.ServerPathExplorerScavengerHuntClueAddUnitList"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.PlayerPath.ServerPathUnlockResult"] = "the client takes the unlocked path mask in 4 bits where the seed wrote 64; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerMapTrackedUnitUpdate"] = "the client takes the public event objective id in 15 bits where the seed wrote 14; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventTriggerUiUpdates"] = "the client takes the event id in 32 bits where the seed wrote 14; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventUnitAdd"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.Reward.LockedReward"] = "the client takes the rewarded flags in 32 bits where the seed wrote 64; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerRewardRotationLockedAdd"] = NestsLockedReward,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerRewardRotationLockedList"] = NestsLockedReward,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerRewardRotationLockedRemove"] = NestsLockedReward,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerRewardRotationLockedUpdate"] = NestsLockedReward,
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupActionResult"] = "the client takes the result in 6 bits where the seed wrote 32; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.ServerItemAdd"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.ServerItemError"] = "the client takes the error code in 32 bits where the seed wrote 64; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.ServerPlayerCreate"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Shared.InventoryItem"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Shared.Item"] = "the legacy Write sends Glyphs.Count as the count of Unknown88; the declaration counts Unknown88",
        ["NexusTomorrow.Network.World.Message.Model.Spell.ClientCastGuildBossToken"] = "the client sends the token item id in 18 bits where the seed read 32; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellGo"] = "the legacy Write puts the sbyte Phase through Write(int), 32 sign-extended bits, where the client facts give the 8 declared",
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellVisualEffectRemove"] = "the client takes the visual effect id in 32 bits where the seed wrote 18; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellVisualEffectRemoveFromUnit"] = "the client takes the target unit id in 32 bits where the seed wrote 18; the declaration follows the client",
        ["NexusTomorrow.Network.World.Message.Model.Support.ClientCustomerSurveySubmit"] = "the legacy Read reads every survey arm with ReadShort(32), which throws, and leaves Survey null for any other id, so the legacy model never reads",
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+ChallengesSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+LevelingSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestDifficultySurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestHoldoutSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestTSpellSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Utility.ServerInspectPlayerResponse"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Utility.ServerPlayedResponse"] = "the legacy Write converts a DateTime with ToFileTime, which no declaration does; the declaration carries the file time",
    };

    /// <summary>
    /// Declared types checked against their legacy Read only, by full name, each with its reason.
    /// </summary>
    public static IReadOnlyDictionary<string, string> ReadOnly { get; } = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["NexusTomorrow.Network.World.Chat.Model.ChatFormatAlien"] = ChatSeedWidths,
        ["NexusTomorrow.Network.World.Chat.Model.ChatFormatProfanity"] = ChatSeedWidths,
        ["NexusTomorrow.Network.World.Message.Model.Shared.CraftStats"] = "the legacy Write packs the 64 bits high byte first where the legacy Read unpacks them low byte first; the declaration follows the Read",
    };

    /// <summary>
    /// Declared column lists the legacy model keeps as one list of records, by the declaring type's full name.
    /// </summary>
    public static IReadOnlyDictionary<string, RowColumns> Rows { get; } = new Dictionary<string, RowColumns>(StringComparer.Ordinal)
    {
        ["NexusTomorrow.Network.World.Message.Model.Abilities.ServerAmpRespecResult"] = new("Results", [("SpecIndexes", "SpecIndex"), ("Results", "Result")], "the legacy model keeps one record per AMP and writes each field as a column"),
        ["NexusTomorrow.Network.World.Message.Model.Friendship.ServerFriendshipLocationUpdate"] = new("FriendLocations", [("FriendshipIds", "FriendshipId"), ("WorldZoneIds", "WorldZoneId")], "the legacy model keeps one record per friend and writes each field as a column"),
        ["NexusTomorrow.Network.World.Message.Model.Housing.ClientHousingDecorUpdate"] = new("DecorUpdates", [("UseServiceTokens", "UseServiceToken")], "the legacy model keeps each update's service token bit on the update and reads the bits as a column after the updates"),
        ["NexusTomorrow.Network.World.Message.Model.Housing.ServerHousingHarvestItemsSentToOwner"] = new("HarvestedItems", [("Item2Ids", "Item2Id"), ("Counts", "Count")], "the legacy model keeps one record per harvested item and writes each field as a column"),
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupPositionUpdate"] = new("Updates", [("Identities", "Identity"), ("Positions", "Position"), ("WorldZoneIds", "WorldZoneId"), ("Flags", "Flags")], "the legacy model keeps one record per member and writes each field as a column"),
        ["NexusTomorrow.Network.World.Message.Model.ServerQuestInit+QuestActive"] = new("Objectives", [("ObjectiveProgress", "Progress"), ("ObjectiveTimeElapsed", "TimeElapsed")], "the legacy model keeps one record per objective and writes each field as a column, with the quest's elapsed time between the two"),
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup"] = new("Categories", [("CategoryIds", "Id"), ("CategoryIndexes", "Index")], "the legacy model keeps one record per category and writes each field as a column"),
    };

    /// <summary>
    /// Adjustments to every filled instance of a declared type, by full name, that keep its values where the legacy model carries them unchanged.
    /// </summary>
    public static IReadOnlyDictionary<string, Rule> Limits { get; } = new Dictionary<string, Rule>(StringComparer.Ordinal)
    {
        ["NexusTomorrow.Network.World.Message.Model.Friendship.ServerFriendshipLocationUpdate"] = new(
            "the legacy record keeps WorldZoneId in a ushort, which it writes in 32 bits",
            value => Mask(((Model.Friendship.ServerFriendshipLocationUpdate)value).WorldZoneIds, ushort.MaxValue)),
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+DiscreteValue"] = new(
            FloatHeldInteger,
            value => ((Model.Reward.ServerPremiumRewards.DiscreteValue)value).Value &= FloatExactIntegers),
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityStatUpdateInteger+IntegerStatValue"] = new(
            FloatHeldInteger,
            value => ((Model.ServerEntityStatUpdateInteger.IntegerStatValue)value).Value &= FloatExactIntegers),
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupMemberStatUpdate"] = new(
            InitialStatList,
            value => Initialize(((Model.ServerGroupMemberStatUpdate)value).SomeStatList)),
        ["NexusTomorrow.Network.World.Message.Model.Shared.GroupCharacter"] = new(
            InitialStatList,
            value => Initialize(((Model.Shared.GroupCharacter)value).SomeStatList)),
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial+DataValue"] = new(
            FloatHeldInteger,
            value => ((Model.Shared.StatValueInitial.DataValue)value).Value &= FloatExactIntegers),
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial+IntegerValue"] = new(
            FloatHeldInteger,
            value => ((Model.Shared.StatValueInitial.IntegerValue)value).Value &= FloatExactIntegers),
    };

    /// <summary>
    /// Off-wire legacy members set on the legacy instance a declared type maps onto, by the declared type's full name.
    /// </summary>
    public static IReadOnlyDictionary<string, Rule> Presets { get; } = new Dictionary<string, Rule>(StringComparer.Ordinal)
    {
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityStatUpdateInteger+IntegerStatValue"] = new(
            "the legacy Write picks the value's encoding from Type, which is not on the wire; the integer opcode's value is the Integer encoding",
            legacy => ((LegacyModel.Shared.StatValueUpdate)legacy).Type = StatType.Integer),
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueUpdate"] = new(
            "the legacy Write picks the value's encoding from Type, which is not on the wire; the declaration is the float opcode's layout",
            legacy => ((LegacyModel.Shared.StatValueUpdate)legacy).Type = StatType.Float),
    };

    /// <summary>
    /// Declared members whose legacy twin has another name, by the declaring type's full name and the member's name.
    /// </summary>
    public static IReadOnlyDictionary<(string Type, string Member), Rename> Renames { get; } = new Dictionary<(string Type, string Member), Rename>
    {
        [("NexusTomorrow.Network.World.Message.Model.ServerEntityStatUpdateInteger", "IntegerStat")] = new(
            "Stat",
            "the legacy integer message derives from the float message and carries its integer value in the inherited Stat"),
    };

    /// <summary>
    /// Pads under the read check, by the declaring type's full name and the pad's name, each with what the legacy Read does with those bits. The generated Write
    /// sends a pad as zeros, so the read check never hands the legacy Read anything else there.
    /// </summary>
    public static IReadOnlyDictionary<(string Type, string Pad), string> ZeroFedPads { get; } = new Dictionary<(string Type, string Pad), string>
    {
        [("NexusTomorrow.Network.World.Message.Model.Marketplace.Filter.EquippableByAuctionFilter", "ClassIdHigh")] = ByteEnumIn14Bits,
        [("NexusTomorrow.Network.World.Message.Model.Marketplace.Filter.EquippableByAuctionFilter", "RaceIdHigh")] = ByteEnumIn14Bits,
        [("NexusTomorrow.Network.World.Message.Model.Option.ClientCombatLogDisableOthers", "Reserved")] = "the legacy Read sets the flag when any of the 32 bits is set, where the declaration reads bit 0 alone; stage 3 declares the client's 32-bit field",
        [("NexusTomorrow.Network.World.Message.Model.Shared.CraftStats", "Reserved")] = "the legacy Read shifts past these 8 bits without keeping them",
        [("NexusTomorrow.Network.World.Message.Model.Who.Parameter.WhoParameterClass", "ClassIdHigh")] = ByteEnumIn14Bits,
        [("NexusTomorrow.Network.World.Message.Model.Who.Parameter.WhoParameterCombo", "ClassIdHigh")] = ByteEnumIn14Bits,
        [("NexusTomorrow.Network.World.Message.Model.Who.Parameter.WhoParameterCombo", "RaceIdHigh")] = ByteEnumIn14Bits,
        [("NexusTomorrow.Network.World.Message.Model.Who.Parameter.WhoParameterRace", "RaceIdHigh")] = ByteEnumIn14Bits,
    };

    /// <summary>
    /// Union holders whose selector the legacy model stores and no legacy Read checks, by full name, each with how its [UnionCase] values were checked instead.
    /// The mapper copies the declared selector, which the generated Write takes from the arm's [UnionCase], onto the stored legacy member, so a legacy Write
    /// that does not branch on it cannot tell a wrong case value.
    /// </summary>
    public static IReadOnlyDictionary<string, string> UncheckedSelectors { get; } = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+RewardProperty"] = ModifierTypeSwitch,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+RewardProperty+RewardOwnerMultiplier"] = ModifierTypeSwitch,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate"] = "the legacy Write sends Type and then the model, without branching; 25 of the 37 arm values are the EntityType the seed's entity classes pair with the model they build (5f7c530b, each class's Type and BuildEntityModel), and the other 12, models the seed never builds, are the EntityType member of the model's name; stage 3 checks those 12 in the client",
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureA8"] = SelectorSwitch,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureC8"] = SelectorSwitch,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+WorldPlacement"] = SelectorSwitch,
        ["NexusTomorrow.Network.World.Message.Model.ServerMatchingPvpKillNotification"] = "the legacy Write picks the player or the creature member of each side by its type, so a wrong case value writes the member the mapping leaves empty and fails the write check",
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup+Offer+OfferItemData"] = SelectorSwitch,
        ["NexusTomorrow.Network.World.Message.Model.Shared.EffectInfo"] = SelectorSwitch,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjectiveStatus"] = SelectorSwitch,
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial"] = "the legacy Write switches on Type between a uint cast, the float's own bits, and a uint cast followed by Data, so a wrong case value writes another encoding or length and fails the write check for any value but zero",
    };

    /// <summary>
    /// The pairing the production check runs with: an excluded union arm is never filled into the unions holding it.
    /// </summary>
    public static Pairing Pairing { get; } = new(TwinOf)
    {
        SkipsArm = arm => arm.FullName is { } fullName && Exclusions.ContainsKey(fullName),
        ReadOnly = ReadOnly,
        Rows = Rows,
        Limits = Limits,
        Presets = Presets,
        Renames = Renames,
    };

    /// <summary>
    /// Every non-abstract type carrying [WireStruct] or [WireMessage] in the declaring assemblies, in full-name order.
    /// </summary>
    public static List<Type> DeclaredTypes()
    {
        return
        [
            .. s_declaringAssemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && WireMembers.IsWireType(type))
                .OrderBy(type => type.FullName, StringComparer.Ordinal),
        ];
    }

    /// <summary>
    /// The legacy type with the full name of <paramref name="declared"/>, or null when the legacy copy has none.
    /// </summary>
    public static Type? TwinOf(Type declared)
    {
        return declared.FullName is { } fullName ? s_legacyAssembly.GetType(fullName, throwOnError: false) : null;
    }

    /// <summary>
    /// The services legacy models take: ClientChat and ClientChatWhisper resolve ChatClientFormat, which resolves the chat format models by type.
    /// </summary>
    public static ServiceProvider CreateLegacyServices()
    {
        var services = new ServiceCollection();
        legacy::NexusTomorrow.Network.World.Chat.ServiceCollectionExtensions.AddNetworkWorldChat(services);
        return services.BuildServiceProvider();
    }

    private static void Mask(List<uint> values, uint mask)
    {
        for (int i = 0; i < values.Count; i++)
            values[i] &= mask;
    }

    private static void Initialize(Model.Shared.GroupCharacter.UnknownStruct0[] stats)
    {
        var initial = new Model.Shared.GroupCharacter.UnknownStruct0();
        foreach (Model.Shared.GroupCharacter.UnknownStruct0 stat in stats)
        {
            stat.Unknown6 = initial.Unknown6;
            stat.Unknown7 = initial.Unknown7;
        }
    }
}
