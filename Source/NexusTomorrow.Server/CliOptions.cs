namespace NexusTomorrow.Server;

/// <summary>
/// The <c>Cli</c> configuration section.
/// </summary>
public sealed class CliOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Cli";

    /// <summary>
    /// Whether the server reads admin commands from its console. The console stays off whenever standard input is redirected.
    /// </summary>
    public bool Enabled { get; set; } = true;
}
