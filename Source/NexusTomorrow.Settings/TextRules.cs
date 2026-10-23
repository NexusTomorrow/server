using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace NexusTomorrow.Settings;

/// <summary>
/// A text setting that must be set to something other than white space.
/// </summary>
public sealed class RequiredSettingAttribute : SettingRuleAttribute
{
    /// <inheritdoc/>
    public override string MustBe(Setting setting) => "Set";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        string.IsNullOrWhiteSpace(value as string) ? $"{setting.Key} is required." : null;
}

/// <summary>
/// A text setting that must be an IPv4 address.
/// </summary>
public sealed class IPv4AddressAttribute : SettingRuleAttribute
{
    /// <inheritdoc/>
    public override string MustBe(Setting setting) => "An IPv4 address";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        IPAddress.TryParse(value as string, out IPAddress? address) && address.AddressFamily == AddressFamily.InterNetwork
            ? null
            : $"{setting.Key} must be an IPv4 address.";
}

/// <summary>
/// A text setting that must be an IPv4 or IPv6 address.
/// </summary>
public sealed class IPAddressAttribute : SettingRuleAttribute
{
    /// <inheritdoc/>
    public override string MustBe(Setting setting) => "An IP address";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value) =>
        IPAddress.TryParse(value as string, out _) ? null : $"{setting.Key} must be an IP address.";
}

/// <summary>
/// A required text setting holding base64 of at least a number of bytes, such as a key. The decoded bytes are zeroed once counted.
/// </summary>
public sealed class Base64BytesAttribute : SettingRuleAttribute
{
    /// <summary>
    /// Require base64 of at least <paramref name="minimumBytes"/> bytes.
    /// </summary>
    public Base64BytesAttribute(int minimumBytes)
    {
        MinimumBytes = minimumBytes;
    }

    /// <summary>
    /// The fewest bytes the text may decode to.
    /// </summary>
    public int MinimumBytes { get; }

    /// <inheritdoc/>
    public override string MustBe(Setting setting) => $"Set: base64 of at least {SettingText.Prose(MinimumBytes)} bytes";

    /// <inheritdoc/>
    public override string? Check(Setting setting, object? value)
    {
        string? text = value as string;
        if (string.IsNullOrEmpty(text))
            return $"{setting.Key} is required: base64 of at least {SettingText.Digits(MinimumBytes)} bytes.";
        return IsBase64OfAtLeast(text, MinimumBytes) ? null : $"{setting.Key} must be base64 of at least {SettingText.Digits(MinimumBytes)} bytes.";
    }

    private static bool IsBase64OfAtLeast(string text, int bytes)
    {
        byte[] buffer = new byte[text.Length];
        try
        {
            return Convert.TryFromBase64String(text, buffer, out int written) && written >= bytes;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(buffer);
        }
    }
}
