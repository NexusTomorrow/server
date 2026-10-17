using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using NexusTomorrow.Gateway;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;

namespace NexusTomorrow.Server;

// Every message below names a configuration key and never its value: a secret or a connection string must stay out of any log.

internal sealed class GatewayOptionsValidator : IValidateOptions<GatewayOptions>
{
    private const int MinimumSecretBytes = 32;

    public ValidateOptionsResult Validate(string? name, GatewayOptions options)
    {
        List<string> failures = [];
        string secretKey = $"{GatewayOptions.Section}:{nameof(GatewayOptions.UnknownAccountSecret)}";
        if (string.IsNullOrEmpty(options.UnknownAccountSecret))
            failures.Add($"{secretKey} is required: base64 of at least {MinimumSecretBytes} bytes.");
        else if (!IsBase64OfAtLeast(options.UnknownAccountSecret, MinimumSecretBytes))
            failures.Add($"{secretKey} must be base64 of at least {MinimumSecretBytes} bytes.");

        string worldKey = $"{GatewayOptions.Section}:{nameof(GatewayOptions.WorldPublicAddress)}";
        if (!IPAddress.TryParse(options.WorldPublicAddress, out IPAddress? world) || world.AddressFamily != AddressFamily.InterNetwork)
            failures.Add($"{worldKey} must be an IPv4 address.");

        CheckListener(nameof(GatewayOptions.Sts), options.Sts, failures);
        CheckListener(nameof(GatewayOptions.Login), options.Login, failures);
        CheckListener(nameof(GatewayOptions.World), options.World, failures);
        int[] ports = [options.Sts.Port, options.Login.Port, options.World.Port];
        if (ports.Distinct().Count() != ports.Length)
            failures.Add($"{GatewayOptions.Section}: the STS, login and world ports must be three different ports.");

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
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

    // DataAnnotations does not descend into a nested options object, so the listener's own attributes are checked here.
    private static void CheckListener(string listener, ListenerOptions options, List<string> failures)
    {
        string key = $"{GatewayOptions.Section}:{listener}";
        if (!IPAddress.TryParse(options.BindAddress, out _))
            failures.Add($"{key}:{nameof(ListenerOptions.BindAddress)} must be an IP address.");

        List<ValidationResult> results = [];
        Validator.TryValidateObject(options, new ValidationContext(options), results, validateAllProperties: true);
        foreach (ValidationResult result in results)
            failures.Add($"{key}: {result.ErrorMessage}");
    }
}

internal sealed class PersistenceOptionsValidator : IValidateOptions<PersistenceOptions>
{
    public ValidateOptionsResult Validate(string? name, PersistenceOptions options)
    {
        List<string> failures = [];
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            failures.Add($"{PersistenceOptions.Section}:{nameof(PersistenceOptions.ConnectionString)} is required.");
        if (options.SaveWorkers < 1)
            failures.Add($"{PersistenceOptions.Section}:{nameof(PersistenceOptions.SaveWorkers)} must be at least 1.");

        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);
    }
}

internal sealed class RealmOptionsValidator : IValidateOptions<RealmOptions>
{
    public ValidateOptionsResult Validate(string? name, RealmOptions options) =>
        string.IsNullOrWhiteSpace(options.Name)
            ? ValidateOptionsResult.Fail($"{RealmOptions.Section}:{nameof(RealmOptions.Name)} is required.")
            : ValidateOptionsResult.Success;
}
