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
    private const string Columns = "the legacy model keeps rows and writes them as columns; the declaration names the columns, which the name mapping cannot follow";
    private const string DefaultStatList = "the legacy Write replaces SomeStatList with five default elements before writing it, so it never writes the values it holds";
    private const string FloatOrInteger = "the legacy model writes one float as a float or as a uint cast by a switch on its type; the declaration holds the wire value in float and integer arms, which the name mapping cannot follow";
    private const string NestsEffectInfo = "nests EffectInfo, whose payload union the name mapping cannot follow";
    private const string NestsGroupCharacter = "nests GroupCharacter, whose legacy Write replaces SomeStatList with five default elements";
    private const string NestsObjectiveStatus = "nests PublicEventObjectiveStatus, whose payload union the name mapping cannot follow";
    private const string SwitchArm = "an arm of a union the legacy model writes as a switch over flat members, so the legacy copy has no type for it";
    private const string SwitchHolder = "the legacy model writes this payload as a switch over flat members; the declaration holds it as a union member, which the name mapping cannot follow";

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
        ["NexusTomorrow.Network.World.Message.Model.Cinematic.ClientCinematicCameraSubjectPosVel"] = "the legacy Read calls Read on a Position it never creates, so it always throws; the declaration creates it",
        ["NexusTomorrow.Network.World.Message.Model.ClientEntityCommand"] = "nests the four command arms whose legacy Read takes a float through ReadUInt, a numeric conversion; every difference is in those arms",
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventObjectiveStatusUpdate"] = NestsObjectiveStatus,
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventObjectiveUpdate"] = NestsObjectiveStatus,
        ["NexusTomorrow.Network.World.Message.Model.PublicEvent.ServerPublicEventStart"] = NestsObjectiveStatus,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards"] = "nests RewardProperty, whose legacy model writes its float Value as a float or a uint cast",
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+AdditiveScalarValue"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+DiscreteValue"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+MultiplicativeScalarValue"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+RewardProperty"] = FloatOrInteger,
        ["NexusTomorrow.Network.World.Message.Model.Reward.ServerPremiumRewards+RewardProperty+RewardOwnerMultiplier"] = FloatOrInteger,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCommand"] = "nests SetPositionMultiSplineCommand, whose legacy Write puts Speed in 32 bits where the declaration takes the Read's 16; every difference is in that arm",
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate"] = "nests SetPositionMultiSplineCommand in Commands, and payload unions the name mapping cannot follow: StatValueInitial, EffectInfo, UnknownStructureA8, WorldPlacement and UnknownStructureC8",
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureA8"] = SwitchHolder,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureA8+Flag"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureA8+Value"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureC8"] = SwitchHolder,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureC8+Flag"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+UnknownStructureC8+Values"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+WorldPlacement"] = SwitchHolder,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+WorldPlacement+Flag"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityCreate+WorldPlacement+Socket"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityStatUpdateFloat"] = "nests StatValueUpdate, whose legacy Write picks the value's encoding from Type, which is not on the wire",
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityStatUpdateInteger"] = "the legacy model derives from ServerEntityStatUpdateFloat; the declaration carries the integer value in IntegerStat, which the name mapping cannot follow",
        ["NexusTomorrow.Network.World.Message.Model.ServerEntityStatUpdateInteger+IntegerStatValue"] = "the integer opcode's stat layout; the legacy model derives from ServerEntityStatUpdateFloat and has no type for it",
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupInviteReceived"] = NestsGroupCharacter,
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupJoin"] = NestsGroupCharacter,
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupMemberAdd"] = NestsGroupCharacter,
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupMemberStatUpdate"] = DefaultStatList,
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupPositionUpdate"] = Columns,
        ["NexusTomorrow.Network.World.Message.Model.ServerGroupRequestJoinResponse"] = NestsGroupCharacter,
        ["NexusTomorrow.Network.World.Message.Model.ServerMatchingPvpKillNotification"] = "the legacy model keeps a player and a creature member for each side and writes one by type; the declaration holds each side as one union member, which the name mapping cannot follow",
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers"] = "nests OfferGroup, whose categories are columns, and OfferItemData, whose payload union the name mapping cannot follow",
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup"] = Columns,
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup+Offer"] = "nests OfferItemData, whose payload union the name mapping cannot follow",
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup+Offer+OfferItemData"] = SwitchHolder,
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup+Offer+OfferItemData+AccountItem"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup+Offer+OfferItemData+UnknownPayload1"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.ServerStoreOffers+OfferGroup+Offer+OfferItemData+UnknownPayload2"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.EffectInfo"] = SwitchHolder,
        ["NexusTomorrow.Network.World.Message.Model.Shared.EffectInfo+NoResult"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.EffectInfo+Result"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.Group"] = NestsGroupCharacter,
        ["NexusTomorrow.Network.World.Message.Model.Shared.GroupCharacter"] = DefaultStatList,
        ["NexusTomorrow.Network.World.Message.Model.Shared.GroupMember"] = NestsGroupCharacter,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjective"] = NestsObjectiveStatus,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjectiveStatus"] = SwitchHolder,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjectiveStatus+CapturePointData"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjectiveStatus+CapturePointDefendData"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjectiveStatus+EmptyData"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.PublicEventObjectiveStatus+VirtualItemDepotData"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial"] = FloatOrInteger,
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial+DataValue"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial+FloatValue"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueInitial+IntegerValue"] = SwitchArm,
        ["NexusTomorrow.Network.World.Message.Model.Shared.StatValueUpdate"] = "the legacy Write picks a float or a uint cast from Type, which is not on the wire; the declaration is the float opcode's layout",
        ["NexusTomorrow.Network.World.Message.Model.Shared.TargetInfo"] = NestsEffectInfo,
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellExecute"] = NestsEffectInfo,
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellGo"] = NestsEffectInfo,
        ["NexusTomorrow.Network.World.Message.Model.Spell.ServerSpellList"] = NestsEffectInfo,
        ["NexusTomorrow.Network.World.Message.Model.Spell.SpellInit"] = NestsEffectInfo,
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
