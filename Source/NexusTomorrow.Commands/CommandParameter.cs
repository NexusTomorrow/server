namespace NexusTomorrow.Commands;

/// <summary>
/// One parameter of a command. A secret parameter's value reaches neither the audit nor a log. The rest parameter, which must be the
/// last, takes every remaining token joined by single spaces.
/// </summary>
public sealed record CommandParameter(string Name, string Description, bool Secret = false, bool Optional = false, bool Rest = false);
