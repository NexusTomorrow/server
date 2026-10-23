namespace NexusTomorrow.Game.Static.Prerequisite;

public enum PrerequisiteType
{
    None = 0,
    Level = 1,
    Race = 2,
    Class = 3,
    Faction = 4,
    Reputation = 5,
    QuestState = 6,
    AchievementState = 7,
    ItemProficiency = 8,
    EpisodeState = 9,
    Gender = 10,
    OtherPrerequisite = 11,
    DeadState = 12,
    ItemEquipped = 13,
    ItemOnCharacter = 14,
    UnderSpell = 15,
    DistanceToWorldLocation = 16,
    // 17 is unused in PrerequisiteType.tbl
    ScheduledEvent = 18,
    TradeSkillProfession = 19,
    GroupSize = 20,
    TradeSkillSchematicLevel = 21,
    QuestObjective0 = 22,
    QuestObjective1 = 23,
    InPhase = 24,
    CanSeePhase = 25,
    InSubZone = 26,
    InTriggerVolume = 27,
    InCombat = 28,
    TimeOfDay = 29,
    FacilityWithinDistance = 30,
    CreatureWithinDistance = 31,
    // 32 is unused in PrerequisiteType.tbl
    QuestObjective2 = 33,
    QuestObjective3 = 34,
    QuestObjective4 = 35,
    QuestObjective5 = 36,
    RandomPercent = 37,
    IsCreature = 38,
    IsPlayer = 39,
    Health = 40,
    ZoneExplored = 41,
    IsGroupLeader = 42,
    IsObjectiveActive = 43,
    InTargetGroup = 44,
    // 45 is unused in PrerequisiteType.tbl
    Waypoint = 46,
    Unknown47 = 47,
    Unknown48 = 48,
    Schedule = 49,
    Unknown50 = 50,
    ItemQuantity = 51,
    Path = 52,
    PathEpisode = 53,
    PlayerPathMission = 54,
    Unknown55 = 55,
    Unknown56 = 56,
    // 57 is unused in PrerequisiteType.tbl
    // 58 is unused in PrerequisiteType.tbl
    Spell59 = 59,
    Spell60 = 60,
    PathMissionCount = 61,
    ScanCreature = 62, // Probably HasScannedCreature
    Unknown63 = 63,
    Unknown64 = 64,
    Deprecated65 = 65, // Marked as DEPRECATED
    Deprecated66 = 66, // Marked as DEPRECATED
    PathMissionRequirement = 67,
    QuestObjective = 68,
    ChallengeRequirement = 69,
    ChallengeTier = 70,
    Unknown71 = 71,
    // 72 is unused in PrerequisiteType.tbl
    Vital = 73, // Part of Vital
    ChallengeLocked = 74,
    // 75 is unused in PrerequisiteType.tbl
    Unknown76 = 76,
    Unknown77 = 77,
    MovementMode = 78,
    UnderForcedMovement = 79,
    HealthRequirement = 80,
    WrongSpellMechanic = 81,
    Unknown82 = 82,
    Vehicle = 83,
    Mount = 84,
    Taxi = 85,
    Jump = 86,
    Moving = 87,
    Pet = 88,
    Unknown89 = 89,
    Unknown90 = 90,
    PathLevel = 91,
    SpellCondition92 = 92,
    SpellCondition93 = 93,
    QuestObjective6 = 94,
    Distance = 95,
    Unknown96 = 96,
    Unknown97 = 97,
    Unknown98 = 98,
    Unknown99 = 99,
    Unknown100 = 100,
    Unknown101 = 101,
    SpellEffect102 = 102,
    SpellEffect103 = 103,
    SpellEffect104 = 104,
    SpellEffect105 = 105,
    Unknown106 = 106,
    QuestObjective7 = 107,
    /// <summary>
    /// Checks to see if a PositionalRequirement Entry is met.
    /// </summary>
    PositionalRequirement = 108,
    WorldRequirement = 109,
    PlayerPathMissionCount = 110,
    HazardProperty111 = 111,
    HazardProperty112 = 112,
    HazardProperty113 = 113,
    Vital114 = 114,
    Unit = 115,
    Stealth = 116,
    PublicEvent117 = 117,
    PublicEventObjective188 = 118,
    PublicEventObjectiveObject = 119,
    PublicEvent120 = 120,
    PublicEvent121 = 121,
    PublicEventObjective122 = 122,
    Unknown123 = 123,
    ChallengeCompletionCount = 124,
    Challenge125 = 125,
    PathHoldout = 126,
    PathHoldoutPlayer = 127,
    Faction128 = 128,
    SpellObj = 129,
    Spell130 = 130,
    Level131 = 131,
    // 132 is unused in PrerequisiteType.tbl
    Item133 = 133,
    Item134 = 134,
    Item135 = 135,
    Item136 = 136,
    ItemLevel = 137,
    ItemSpecial = 138,
    ItemMicrochip = 139,
    Item140 = 140,
    IsOutOfBounds = 141,
    Unknown142 = 142,
    Difficulty = 143,
    Unknown144 = 144, // Exist in PrerequisiteType.tbl but does not have a description
    ItemIsSelfCraftedWeapon = 145,
    ItemTradeSkillLevel = 146,
    PlayersInWorld = 147,
    InfrastructureState = 148,
    State = 149,
    HubEconomyProgress = 150,
    HubQualityOfLife = 151,
    HubSecurity = 152,
    HoldoutWave = 153,
    Holdout = 154,
    PathMission155 = 155,
    // 156 is unused in PrerequisiteType.tbl
    // 157 is unused in PrerequisiteType.tbl
    // 158 is unused in PrerequisiteType.tbl
    Unknown159 = 159,
    Challenge160 = 160,
    // 161 is unused in PrerequisiteType.tbl
    PublicEventObjective2 = 162,
    PublicEventParticipantCount = 163,
    PublicEventParticipant164 = 164,
    // 165 is unused in PrerequisiteType.tbl
    PublicEventParticipant166 = 166,
    LiveEvent167 = 167,
    LiveEventCount = 168,
    LiveEvent169 = 169,
    Unknown170 = 170,
    Unknown171 = 171,
    Unknown172 = 172,
    ItemTradeSkill = 173,
    Item6 = 174,
    TradeSkill = 175,
    ChallengeObject = 176,
    TrueLevel = 177,
    Item7 = 178, // Item of type Equipped?
    Unknown179 = 179,
    Unknown180 = 180,
    // 181 is unused in PrerequisiteType.tbl
    HouseOwnership = 182,
    Guild = 183,
    Guild2 = 184,
    GuildPerk = 185,
    Unknown186 = 186,
    WarplotPlugUpgrade = 187,
    WarplotPermission = 188,
    Unknown189 = 189, // Exist in PrerequisiteType.tbl but does not have a description
    PetFlair = 190,
    Spell191 = 191,
    Unknown192 = 192, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown193 = 193,
    MountUsage = 194,
    HoverboardUsage = 195,
    Racial = 196,
    GliderUsage = 197,
    SpaceshipUsage = 198,
    PublicEvent199 = 199,
    PublicEventObjective200 = 200,
    AchievementObject = 201,
    // 202 is unused in PrerequisiteType.tbl
    Unknown203 = 203, // Exist in PrerequisiteType.tbl but does not have a description
    Dueling = 204,
    Personal2V2ArenaRating205 = 205,
    Personal3V3ArenaRating206 = 206,
    Personal5V5ArenaRating207 = 207,
    Battlegrounds208 = 208,
    Warplots209 = 209,
    Personal2V2ArenaRating210 = 210,
    Personal3V3ArenaRating211 = 211,
    Personal5V5ArenaRating212 = 212,
    PersonalWarplotRating = 213,
    SpellBaseId = 214,
    Shield215 = 215,
    Shield216 = 216,
    PvpFlag = 217,
    Datacube = 218,
    Volume = 219,
    Unknown220 = 220, // Exist in PrerequisiteType.tbl but does not have a description
    Spell221 = 221, // Could be SpellBase
    Unknown222 = 222,
    Unknown223 = 223,
    LevelGrantAbilityTierPoints = 224,
    LevelGrantAmpPoints = 225,
    Entitlement = 226,
    EldanAugmentation227 = 227,
    TradeSkill228 = 228,
    TradeSkill229 = 229,
    WarplotRating = 230,
    EldanAugmentation231 = 231,
    Plane = 232,
    Spell233 = 233,
    WarplotUpgrade = 234,
    BonusAbilityTierPoints = 235,
    AbilityTierPoints = 236,
    BonusPower = 237,
    Power = 238,
    ActionBar = 239,
    Unknown240 = 240, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown241 = 241, // Exist in PrerequisiteType.tbl but does not have a description
    LiveEvent242 = 242,
    Faction243 = 243,
    Unknown244 = 244, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown245 = 245, // Exist in PrerequisiteType.tbl but does not have a description
    Inventory = 246, // AccountItemClaimed?
    // 247 is unused in PrerequisiteType.tbl
    Unknown248 = 248, // Exist in PrerequisiteType.tbl but does not have a description
    // 249 is unused in PrerequisiteType.tbl
    BaseFaction = 250,
    Personal2V2ArenaRating251 = 251,
    Personal3V3ArenaRating252 = 252,
    Personal5V5ArenaRating253 = 253,
    Battlegrounds254 = 254,
    Warplots255 = 255,
    WarplotRating256 = 256,
    // 257 is unused in PrerequisiteType.tbl
    // 258 is unused in PrerequisiteType.tbl
    Unknown259 = 259, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown260 = 260,
    Personal2V2ArenaRating261 = 261,
    Personal3V3ArenaRating262 = 262,
    Personal5V5ArenaRating263 = 263,
    Battlegrounds264 = 264,
    Warplots265 = 265,
    Unknown266 = 266, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown267 = 267, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown268 = 268, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown269 = 269, // Exist in PrerequisiteType.tbl but does not have a description
    LoyaltyRewards = 270,
    Unknown271 = 271, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown272 = 272, // Exist in PrerequisiteType.tbl but does not have a description
    EntitlementCount = 273,
    // 274 is unused in PrerequisiteType.tbl
    Unknown275 = 275, // Exist in PrerequisiteType.tbl but does not have a description
    OutOfBounds = 276,
    Unknown277 = 277,
    Unknown278 = 278,
    Spell279 = 279,
    Spell280 = 280,
    Falling = 281,
    // 282 is unused in PrerequisiteType.tbl
    Unknown283 = 283, // Marked as N/A
    // 284 is unused in PrerequisiteType.tbl
    Unknown285 = 285, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown286 = 286, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown287 = 287, // Exist in PrerequisiteType.tbl but does not have a description
    PurchasedTitle = 288,
    Unknown289 = 289, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown290 = 290, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown291 = 291, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown292 = 292, // Exist in PrerequisiteType.tbl but does not have a description - Primal Matrix ?
    Unknown293 = 293, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown294 = 294, // Exist in PrerequisiteType.tbl but does not have a description
    Unknown295 = 295
}
