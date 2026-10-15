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
    private const string ChatSeedWidths = "the legacy Read takes the random text seed in 14 bits and the legacy Write puts it in 32, so the two disagree; the declaration follows the Read";
    private const string HoldsChatSeed = "the legacy Write puts the Alien and Profanity formats' random text seed in 32 bits where their legacy Read takes 14, so no declaration matches both";
    private const string HoldsBareActors = "the legacy StoryMessage holds bare actors and computes each one's 3-bit type from the arm, so the declared actor holders have no twin";

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
        ["NexusTomorrow.Network.World.Chat.Model.ChatFormatAlien"] = ChatSeedWidths,
        ["NexusTomorrow.Network.World.Chat.Model.ChatFormatProfanity"] = ChatSeedWidths,
        ["NexusTomorrow.Network.World.Message.Model.Chat.ChatClientFormat"] = HoldsChatSeed,
        ["NexusTomorrow.Network.World.Message.Model.Chat.ChatFormat"] = HoldsChatSeed,
        ["NexusTomorrow.Network.World.Message.Model.Cinematic.ClientCinematicCameraSubjectPosVel"] = "the legacy Read calls Read on a Position it never creates, so it always throws; the declaration creates it",
        ["NexusTomorrow.Network.World.Message.Model.Marketplace.ClientAuctionsByFilterRequest"] = "the legacy Read keeps only each filter's arm and drops its 3-bit type, so its Filters hold no AuctionFilter for the declared ones to match",
        ["NexusTomorrow.Network.World.Message.Model.ServerCombatLog"] = "the legacy model computes the 6-bit type from the arm and holds no member for it, so the declared selector has no twin",
        ["NexusTomorrow.Network.World.Message.Model.ServerQuestLuaEvent"] = "the legacy Write puts LuaEventId in 16 bits where the client facts give 14, and holds bare arms with each 32-bit type computed from the arm",
        ["NexusTomorrow.Network.World.Message.Model.ServerQuestLuaEvent+LuaEvent"] = "the declaration's holder for one event's 32-bit type and data; the legacy model holds bare arms, so it has no twin",
        ["NexusTomorrow.Network.World.Message.Model.Story.Message.StoryMessage"] = HoldsBareActors,
        ["NexusTomorrow.Network.World.Message.Model.Story.Message.StoryMessage+ActorEntry"] = HoldsBareActors,
        ["NexusTomorrow.Network.World.Message.Model.Support.ClientCustomerSurveySubmit"] = "the legacy Read reads every survey arm with ReadShort(32), which throws, and leaves Survey null for any other id, so the legacy model never reads",
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+ChallengesSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+LevelingSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestDifficultySurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestHoldoutSurvey"] = ReadShortThrows,
        ["NexusTomorrow.Network.World.Message.Model.Support.Survey+QuestTSpellSurvey"] = ReadShortThrows,
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
