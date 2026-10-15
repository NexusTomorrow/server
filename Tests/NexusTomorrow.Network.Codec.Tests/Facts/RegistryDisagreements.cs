namespace NexusTomorrow.Network.Codec.Tests.Facts;

/// <summary>
/// Every declared message that disagrees with the client facts, keyed by the opcode and side each problem names (the text before its colon), each with
/// its reason. The registry check fails on a disagreement not listed here and on a listed one that no longer occurs, so the list only shrinks as stage 3
/// settles each one.
/// </summary>
internal static class RegistryDisagreements
{
    private const string BitsDiffer = "unsettled: the declaration puts a different number of bits on the wire than the client takes";
    private const string FieldsDiffer = "unsettled: the bits agree, but the client takes them as fields of other widths";
    private const string Reordered = "unsettled: the client takes these fields in another order";
    private const string PartMissing = "unsettled: the client takes a part the declaration lacks";
    private const string NoRow = "unsettled: the client has no message with this opcode";
    private const string WrongDirection = "unsettled: the client does not handle the message in this direction";
    private const string NoArmZero = "unsettled: no union arm answers selector 0, so the empty path is incomplete";

    public static IReadOnlyDictionary<string, string> Pinned { get; } = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["0x0094 client to server"] = Reordered, // ClientAuctionCancel
        ["0x00BA server to client"] = FieldsDiffer, // ServerBuybackItemUpdated
        ["0x00C2 client to server"] = BitsDiffer, // ClientCastServiceToken
        ["0x00C6 server to client"] = BitsDiffer, // ServerChallengeResult
        ["0x00D3 server to client"] = BitsDiffer, // ServerHideInstanceGameModeDialog
        ["0x00D6 server to client"] = FieldsDiffer, // ServerPublicEventEnd
        ["0x00D8 server to client"] = FieldsDiffer, // ServerCostume
        ["0x00DC server to client"] = BitsDiffer, // ServerCharacterCreate
        ["0x0100 server to client"] = BitsDiffer, // ServerResurrectionState
        ["0x0103 client to server"] = BitsDiffer, // ClientGalacticArchiveUnlock
        ["0x0104 server to client"] = BitsDiffer, // ServerGalacticArchiveUpdate
        ["0x0105 client to server"] = BitsDiffer, // ClientGalacticArchiveViewed
        ["0x0112 server to client"] = BitsDiffer, // ServerPublicEventStart
        ["0x012E server to client"] = FieldsDiffer, // ServerPetCustomisationList
        ["0x012F server to client"] = FieldsDiffer, // ServerPetCustomisation
        ["0x0131 server to client"] = BitsDiffer, // ServerPublicEventLocationUpdate
        ["0x0133 server to client"] = BitsDiffer, // ServerPublicEventObjectiveNotificationMode
        ["0x0137 server to client"] = BitsDiffer, // ServerPublicEventPersonalStatsUpdate
        ["0x0138 server to client"] = BitsDiffer, // ServerPublicEventMapRegionUpdate
        ["0x0144 client to server"] = PartMissing, // ClientCharacterAppearanceChange
        ["0x014B server to client"] = BitsDiffer, // ServerCharacterRenameResult
        ["0x0154 server to client"] = BitsDiffer, // ServerInstanceResetFailed
        ["0x0158 server to client"] = BitsDiffer, // ServerShowInstanceRestrictedDialog
        ["0x015A client to server"] = BitsDiffer, // ClientResurrectAccept
        ["0x016A client to server"] = BitsDiffer, // ClientNonSpellActionSetChanges
        ["0x0172 server to client"] = BitsDiffer, // ServerPlaneVisibilitySpellSlingerVoid
        ["0x018E server to client"] = FieldsDiffer, // ServerP2PTradeUpdateItem
        ["0x0194 client to server"] = BitsDiffer, // ClientP2PTradingRemoveItem
        ["0x0197 client to server"] = BitsDiffer, // ClientP2PTradingSetMoney
        ["0x0198 server to client"] = Reordered, // ServerP2PTradeUpdateMoney
        ["0x01B7 server to client"] = BitsDiffer, // ServerChatFlag
        ["0x01C0 server to client"] = BitsDiffer, // ServerChatList
        ["0x0231 server to client"] = BitsDiffer, // ServerCinematicActorMode
        ["0x023F client to server"] = FieldsDiffer, // ClientStatisticsConnection
        ["0x0240 client to server"] = FieldsDiffer, // ClientStatisticsFramerate
        ["0x0248 client to server"] = FieldsDiffer, // ClientCombatLogDisableOthers
        ["0x0252 server to client"] = FieldsDiffer, // ServerQuestContractGoodQualityChanged
        ["0x0255 client to server"] = FieldsDiffer, // ClientCostumeSave
        ["0x025B client to server"] = PartMissing, // ClientCharacterCreate
        ["0x025E server to client"] = FieldsDiffer, // ServerPlayerCreate
        ["0x0263 server to client"] = BitsDiffer, // ServerSpellVisualAdd
        ["0x033F"] = NoArmZero, // ClientCustomerSurveySubmit
        ["0x033F client to server"] = "unsettled: the declaration puts a different number of bits on the wire than the client takes, and lacks the client's wide string", // ClientCustomerSurveySubmit
        ["0x0358 server to client"] = BitsDiffer, // ServerDialogEnd
        ["0x0359 server to client"] = BitsDiffer, // ServerQuestLuaEvent
        ["0x0366 server to client"] = BitsDiffer, // ServerDisabledGameplaySystems
        ["0x03A4 server to client"] = BitsDiffer, // ServerFriendshipAccountCharacterZoneChanged
        ["0x03AE server to client"] = BitsDiffer, // ServerFriendshipAccountPrivateNote
        ["0x03B3 server to client"] = WrongDirection, // ClientFriendshipAddByIdentity
        ["0x03BD client to server"] = BitsDiffer, // ClientFriendshipInviteMarkSeen
        ["0x03D1 server to client"] = FieldsDiffer, // ServerFortuneCards
        ["0x03DB server to client"] = FieldsDiffer, // ServerRealmInfo
        ["0x03EE client to server"] = BitsDiffer, // ClientGetRealmTransferDestinations
        ["0x0405 server to client"] = FieldsDiffer, // ServerGroupMemberAdd
        ["0x0424 server to client"] = FieldsDiffer, // ServerGroupRequestJoinResponse
        ["0x046F server to client"] = BitsDiffer, // ServerGuildBankInventoryAdd
        ["0x0487 server to client"] = BitsDiffer, // ServerGuildEventLogChange
        ["0x0493 server to client"] = FieldsDiffer, // ServerGuildJoin
        ["0x04A1 server to client"] = FieldsDiffer, // ServerGuildBankWithdraw
        ["0x04A3 server to client"] = FieldsDiffer, // ServerGuildMemberChange
        ["0x050B client to server"] = PartMissing, // ClientHousingDecorUpdate
        ["0x050D client to server"] = FieldsDiffer, // ClientHousingChangeInteriorWallpaper
        ["0x0510 client to server"] = FieldsDiffer, // ClientHousingPlugUpdate
        ["0x0511"] = NoRow, // ClientHousingNeighbourInviteResponse
        ["0x058F client to server"] = FieldsDiffer, // ClientHelloRealm
        ["0x0592 client to server"] = FieldsDiffer, // ClientHelloAuth
        ["0x0599 server to client"] = FieldsDiffer, // ServerPlayerInfoFullResponse
        ["0x05D9 server to client"] = FieldsDiffer, // ServerMatchingPenaltyUpdated
        ["0x05E3 server to client"] = FieldsDiffer, // ServerMatchingQueueStatus
        ["0x05E5 server to client"] = FieldsDiffer, // ServerMatchingMatchPvpPoolUpdated
        ["0x05E9 server to client"] = FieldsDiffer, // ServerMatchingMatchPvpFinished
        ["0x05EE server to client"] = FieldsDiffer, // ServerMatchingPvpTeamInfoUpdate
        ["0x0667 server to client"] = FieldsDiffer, // ServerPathScientistExperimentationInvoke
        ["0x066A client to server"] = FieldsDiffer, // ClientPathMissionAttemptScientistExperimentation
        ["0x06BC server to client"] = FieldsDiffer, // ServerPathInitialise
        ["0x0768 server to client"] = FieldsDiffer, // ServerRecruitmentGuildDemands
        ["0x076A server to client"] = FieldsDiffer, // ServerRecruitmentGuildDetails
        ["0x07A1 server to client"] = FieldsDiffer, // ServerNewRealm
        ["0x07CD server to client"] = FieldsDiffer, // ServerRewardTrackAdd
        ["0x07D5 server to client"] = FieldsDiffer, // ServerRewardTrackUpdate
        ["0x0823 client to server"] = PartMissing, // ClientSteamAchievements
        ["0x082A client to server"] = BitsDiffer, // ClientStorefrontPurchaseCharacter
        ["0x084F client to server"] = FieldsDiffer, // ClientCraftingComplexCraft
        ["0x0851 client to server"] = BitsDiffer, // ClientCraftingCraftItem
        ["0x0852 client to server"] = BitsDiffer, // ClientCraftingCraftItemAutoCraft
        ["0x0854 server to client"] = FieldsDiffer, // ServerCraftingCurrentCraft
        ["0x0860 server to client"] = FieldsDiffer, // ServerProfessionUpdate
        ["0x08B7 server to client"] = FieldsDiffer, // ServerPathSettlerHubUpdate
        ["0x0909 server to client"] = FieldsDiffer, // ServerEntityThreatListUpdate
        ["0x0954 server to client"] = BitsDiffer, // ServerWarPartyMatchResults
        ["0x0967 server to client"] = BitsDiffer, // ServerAccountCurrencyGrant
        ["0x776E"] = NoRow, // ClientRecruitmentGuildGetDetailedGuildInfo
    };
}
