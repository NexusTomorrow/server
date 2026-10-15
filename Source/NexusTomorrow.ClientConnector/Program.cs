using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NexusTomorrow.ClientConnector.Configuration;
using NexusTomorrow.ClientConnector.Native;

namespace NexusTomorrow.ClientConnector;

internal static class Program
{
    private const string JsonFile = "config.json";

    [DllImport("kernel32.dll")]
    private static extern bool CreateProcess(
        string lpApplicationName,
        string lpCommandLine,
        IntPtr lpProcessAttributes,
        IntPtr lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        IntPtr lpEnvironment,
        string? lpCurrentDirectory,
        ref STARTUPINFO lpStartupInfo,
        out PROCESS_INFORMATION lpProcessInformation
    );

    public static void Main()
    {
        if (!File.Exists(JsonFile))
        {
            Console.Write("Type in your host name: ");
            string? hostName = Console.ReadLine();

            Console.Write("Type in your language: [en,de]");
            string? language = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(hostName) || string.IsNullOrWhiteSpace(language))
            {
                Console.WriteLine("A host name and a language are both required; nothing was written.");
                return;
            }

            var clientConfig = new ClientConfiguration
            {
                HostName = hostName,
                Language = language
            };

            File.WriteAllText(JsonFile, JsonConvert.SerializeObject(clientConfig));
        }

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(JsonFile, false)
            .Build();

        LaunchClient(ReadConfiguration(configuration));
    }

    /// <summary>
    /// Bind <see cref="ClientConfiguration"/> from the supplied configuration, throwing <see cref="InvalidDataException"/> naming any key that is missing or empty, since the configuration binder does not enforce <c>required</c>.
    /// </summary>
    internal static ClientConfiguration ReadConfiguration(IConfiguration configuration)
    {
        ClientConfiguration config = configuration.Get<ClientConfiguration>()
            ?? throw new InvalidDataException($"{JsonFile} holds no client configuration.");

        if (string.IsNullOrWhiteSpace(config.HostName))
            throw new InvalidDataException($"{JsonFile} has no {nameof(ClientConfiguration.HostName)}.");
        if (string.IsNullOrWhiteSpace(config.Language))
            throw new InvalidDataException($"{JsonFile} has no {nameof(ClientConfiguration.Language)}.");

        return config;
    }

    private static void LaunchClient(ClientConfiguration config)
    {
        var si = new STARTUPINFO();
        var pi = new PROCESS_INFORMATION();

        string client = "WildStar64.exe";
        if (!File.Exists(client))
            client = "WildStar32.exe";

        CreateProcess(client,
            $"/auth {config.HostName} /authNc {config.HostName} /lang {config.Language} /patcher {config.HostName} /SettingsKey WildStar /realmDataCenterId 9",
            IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref si, out pi);
    }
}
