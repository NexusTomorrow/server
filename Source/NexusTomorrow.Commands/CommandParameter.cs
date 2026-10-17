namespace NexusTomorrow.Commands;

/// <summary>
/// One parameter of a command. A secret parameter's value reaches neither the audit nor a log. The rest parameter, which must be the
/// last, takes every remaining token joined by single spaces. When <paramref name="IsValid"/> is set, the dispatcher refuses a line whose
/// value fails it, answering <paramref name="Invalid"/>, and records only values that pass it in the audit: a value that fails may
/// be a secret typed in the wrong place.
/// </summary>
public sealed record CommandParameter(
    string Name,
    string Description,
    bool Secret = false,
    bool Optional = false,
    bool Rest = false,
    Func<string, bool>? IsValid = null,
    string? Invalid = null);
