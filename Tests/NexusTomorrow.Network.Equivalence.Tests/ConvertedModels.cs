extern alias legacy;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// The production pairing: every converted model in Network, Network.Auth and Network.World, with its legacy twin of the same full name in the legacy copy.
/// </summary>
internal static class ConvertedModels
{
    private const string ReadShortThrows = "the legacy Read takes its first field with ReadShort(32), which throws, so the legacy model never reads; stage 3 declares it from the client facts";
    private const string Columns = "the legacy model keeps one list of records and writes each of their fields as a column; the declaration holds the columns as lists sharing a count, which have no legacy twin";
    private const string NoCount = "the legacy Write leaves out the 32-bit count the client facts put before the elements; the declaration writes it";
    private const string NestsItem = "it nests Item, whose legacy Write sends Glyphs.Count as the count of Unknown88";

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
        ["NexusTomorrow.Network.NetworkGuid"] = "the legacy copy has no twin, as the legacy models nest this type itself; ClientHelloAuth's check reads it through its legacy Read",
        ["NexusTomorrow.Network.World.Message.Model.Abilities.ServerAmpRespecResult"] = Columns,
        ["NexusTomorrow.Network.World.Message.Model.Achievement.ClientSteamAchievements"] = "the legacy Read decodes the achievement bytes into an ASCII string, which no declaration holds; the declaration keeps the bytes",
        ["NexusTomorrow.Network.World.Message.Model.Cinematic.ClientCinematicCameraSubjectPosVel"] = "the legacy Read calls Read on a Position it never creates, so it always throws; the declaration creates it",
        ["NexusTomorrow.Network.World.Message.Model.Crafting.ServerCraftingCurrentCraft"] = "it nests CraftStats, whose legacy Write packs its bytes high byte first where its Read unpacks them low byte first",
        ["NexusTomorrow.Network.World.Message.Model.Friendship.ServerFriendshipLocationUpdate"] = Columns,
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildBankInventoryAdd"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerGuildBankTabInventory"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.Guild.ServerRecruitmentGuildsUpdate+RecruitmentGuild"] = "the legacy copy has no twin, as the seed writes these fields of a RecruitmentGuildInfo inline; ServerRecruitmentGuildsUpdate's check maps them onto that type",
        ["NexusTomorrow.Network.World.Message.Model.Housing.ServerHousingHarvestItemsSentToOwner"] = Columns,
        ["NexusTomorrow.Network.World.Message.Model.Map.ServerZoneMap"] = "the legacy Write sends Count / 8 as the byte length, from a Count the caller sets apart from the bytes; the declaration sends the bytes' own length",
        ["NexusTomorrow.Network.World.Message.Model.PlayerPath.MissionProgress"] = "the legacy copy has no twin, as the seed writes these four fields of a Mission inline; the mission update and episode progress checks map them onto that type",
        ["NexusTomorrow.Network.World.Message.Model.PlayerPath.ServerPathExplorerScavengerHuntClueAddUnitList"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventUnitAdd"] = NoCount,
        ["NexusTomorrow.Network.World.Message.Model.ServerItemAdd"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.ServerPlayerCreate"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.ServerQuestInit"] = "it nests QuestActive, whose objective columns have no legacy twin",
        ["NexusTomorrow.Network.World.Message.Model.ServerQuestInit+QuestActive"] = Columns,
        ["NexusTomorrow.Network.World.Message.Model.Shared.CraftStats"] = "the legacy Write packs the 64 bits high byte first where the legacy Read unpacks them low byte first; the declaration follows the Read",
        ["NexusTomorrow.Network.World.Message.Model.Shared.InventoryItem"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Shared.Item"] = "the legacy Write sends Glyphs.Count as the count of Unknown88; the declaration counts Unknown88",
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+ChallengesSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+LevelingSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestDifficultySurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestHoldoutSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestTSpellSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Utility.ServerInspectPlayerResponse"] = NestsItem,
        ["NexusTomorrow.Network.World.Message.Model.Utility.ServerPlayedResponse"] = "the legacy Write converts a DateTime with ToFileTime, which no declaration does; the declaration carries the file time",
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
}
