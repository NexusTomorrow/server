namespace NexusTomorrow.Game.Static.AccountInventory;

public enum AccountCurrencyType
{
    None = 0,
    Credd = 1,
    RealmTransfer = 2,

    /// <summary>
    /// Name changes, which the client counts itself from the account items it holds.
    /// </summary>
    NameChange = 3,

    Essence = 4,
    MysticShiny = 5,  // FortuneCoin
    Omnibits = 6,
    NCoins = 7,
    Loyalty = 8,  // CosmicReward
    ServiceToken = 9,
    /* 10 UNUSED */
    Protobucks = 11,
    GiantPoints = 12,
    PromotionToken = 13, // MaxLevelToken
    PromissoryNote = 14,
    RedEssence = 15, // CrimsonEssence
    BlueEssence = 16, // CobaltEssence
    GreenEssence = 17, // ViridianEssence

    /// <summary>
    /// Purple essence, the highest type the client keeps an amount for.
    /// </summary>
    PurpleEssence = 18
}
