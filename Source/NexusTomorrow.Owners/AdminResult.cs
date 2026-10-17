namespace NexusTomorrow.Owners;

/// <summary>
/// What an owner answers an admin command with: whether it did what was asked, and the text to show the operator.
/// </summary>
public sealed record AdminResult(bool Succeeded, string Text);
