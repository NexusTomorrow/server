namespace NexusTomorrow.Game.Static.Option;

/// <summary>
/// Which option the value of an option report carries.
/// </summary>
public enum OptionSetType
{
    /// <summary>
    /// The value is the player's casting options, an <see cref="Option.InputFlags"/> mask.
    /// </summary>
    InputFlags = 0,

    /// <summary>
    /// The value is the player's <see cref="Option.SharedChallengePreference"/>.
    /// </summary>
    SharedChallengePreference = 1,
}
