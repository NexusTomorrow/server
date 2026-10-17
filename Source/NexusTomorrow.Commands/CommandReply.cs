namespace NexusTomorrow.Commands;

/// <summary>
/// How a resolved command ended.
/// </summary>
public enum CommandOutcome
{
    /// <summary>
    /// It did what was asked.
    /// </summary>
    Done,

    /// <summary>
    /// It was understood and declined: bad arguments, or a request the state does not allow.
    /// </summary>
    Refused,

    /// <summary>
    /// The actor lacks the permission, or the command is console only and the actor is in game.
    /// </summary>
    Denied,

    /// <summary>
    /// It threw.
    /// </summary>
    Failed,
}

/// <summary>
/// The answer to a command: its outcome and the text to show the operator.
/// </summary>
public sealed record CommandReply(CommandOutcome Outcome, string Text);
