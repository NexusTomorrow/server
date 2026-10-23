using System.Net;
using Microsoft.Extensions.Options;

namespace NexusTomorrow.Tools;

/// <summary>
/// The <c>Tools:Oracle</c> configuration section: the oracle's endpoint, which exists only in Debug builds and is off until enabled.
/// </summary>
/// <remarks>
/// Not a settings section: the host settings page documents what a Release build reads, and this section is read only in Debug
/// builds, so <see cref="OracleOptionsValidator"/> checks it.
/// </remarks>
public sealed class OracleOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Tools:Oracle";

    /// <summary>
    /// Whether the endpoint listens. Off by default, in Debug builds too.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The address the endpoint binds; it must be a loopback address.
    /// </summary>
    public string Address { get; set; } = "127.0.0.1";

    /// <summary>
    /// The TCP port the endpoint binds.
    /// </summary>
    public int Port { get; set; } = 23900;

    /// <summary>
    /// The file the endpoint writes its token to at each start, under the user's local application data unless set.
    /// </summary>
    public string TokenFile { get; set; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NexusTomorrow", "oracle", "token");
}

/// <summary>
/// Validates <see cref="OracleOptions"/> at start: an enabled endpoint binds a loopback address, a port in range, and writes its token
/// to a rooted path.
/// </summary>
public sealed class OracleOptionsValidator : IValidateOptions<OracleOptions>
{
    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, OracleOptions options)
    {
        if (!options.Enabled)
            return ValidateOptionsResult.Success;

        List<string> failures = [];
        if (!IPAddress.TryParse(options.Address, out IPAddress? address) || !IPAddress.IsLoopback(address))
            failures.Add($"{OracleOptions.Section}:{nameof(OracleOptions.Address)} must be a loopback address.");
        if (options.Port is < 1 or > 65535)
            failures.Add($"{OracleOptions.Section}:{nameof(OracleOptions.Port)} must be from 1 to 65535.");
        if (string.IsNullOrWhiteSpace(options.TokenFile) || !Path.IsPathRooted(options.TokenFile))
            failures.Add($"{OracleOptions.Section}:{nameof(OracleOptions.TokenFile)} must be a rooted path.");

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}
