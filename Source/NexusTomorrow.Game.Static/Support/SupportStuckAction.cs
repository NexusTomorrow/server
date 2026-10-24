namespace NexusTomorrow.Game.Static.Support;

/// <summary>
/// A recall a stuck player can ask for, as GameLib.SupportStuckAction names them; the client keeps a cooldown for each.
/// </summary>
public enum SupportStuckAction
{
    /// <summary>
    /// Recall to the player's bind point; the stuck window offers it only when the player has one.
    /// </summary>
    RecallBind = 1,

    /// <summary>
    /// Recall to the player's house; the stuck window offers it only to a residence owner.
    /// </summary>
    RecallHouse = 2,

    /// <summary>
    /// Recall by the player's death, which the stuck window asks the player to confirm first.
    /// </summary>
    RecallDeath = 3,
}
