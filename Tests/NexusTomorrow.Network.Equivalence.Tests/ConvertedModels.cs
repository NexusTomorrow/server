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
    private const uint FloatExactIntegers = (1u << 24) - 1;

    private static readonly Assembly s_legacyAssembly = typeof(legacy::NexusTomorrow.Network.World.ServiceCollectionExtensions).Assembly;

    private static readonly Assembly[] s_declaringAssemblies =
    [
        typeof(GamePacketWriter).Assembly,
        typeof(NexusTomorrow.Network.Auth.ServiceCollectionExtensions).Assembly,
        typeof(NexusTomorrow.Network.World.ServiceCollectionExtensions).Assembly,
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
        ["NexusTomorrow.Network.World.Message.Model.Achievement.ClientSteamAchievements"] = "the legacy Read decodes the achievement bytes into an ASCII string, which no declaration holds; the declaration keeps the bytes",
        ["NexusTomorrow.Network.World.Message.Model.Cinematic.ClientCinematicCameraSubjectPosVel"] = "the legacy Read calls Read on a Position it never creates, so it always throws; the declaration creates it",
        ["NexusTomorrow.Network.World.Message.Model.Crafting.ServerCraftingCurrentCraft"] = "it nests CraftStats, whose legacy Write packs its bytes high byte first where its Read unpacks them low byte first",
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildBankInventoryAdd"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildBankTabInventory"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.Housing.ClientHousingChangeInteriorWallpaper"] = "the legacy Read calls Read on the six null elements of DecorUpdates, so it always throws; the declaration follows the client facts",
        ["NexusTomorrow.Network.World.Message.Model.Map.ServerZoneMap"] = "the legacy Write sends Count / 8 as the byte length, from a Count the caller sets apart from the bytes; the declaration sends the bytes' own length",
        ["NexusTomorrow.Network.World.Message.Model.PlayerPath.ServerPathExplorerScavengerHuntClueAddUnitList"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventUnitAdd"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.ServerItemAdd"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.ServerPlayerCreate"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Shared.InventoryItem"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Shared.Item"] = "the legacy Write sends Glyphs.Count as the count of Unknown88; the declaration counts Unknown88",
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellGo"] = "the legacy Write puts the sbyte Phase through Write(int), 32 sign-extended bits, where the client facts give the 8 declared",
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
        ["NexusTomorrow.Network.World.Message.Model.Shared.GroupCharacter+UnknownStruct0"] = new(
            "the legacy Write replaces every element of SomeStatList with a new UnknownStruct0 before writing it, so only its initial values reach the wire",
            value =>
            {
                var stat = (Model.Shared.GroupCharacter.UnknownStruct0)value;
                var initial = new Model.Shared.GroupCharacter.UnknownStruct0();
                stat.Unknown6 = initial.Unknown6;
                stat.Unknown7 = initial.Unknown7;
            }),
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
    /// The pairing the production check runs with: a union arm that is excluded or checked one way is never filled into the unions holding it.
    /// </summary>
    public static Pairing Pairing { get; } = new(TwinOf)
    {
        SkipsArm = arm => arm.FullName is { } fullName && (Exclusions.ContainsKey(fullName) || ReadOnly.ContainsKey(fullName)),
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
}
