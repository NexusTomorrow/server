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

    // In order of preference.
    private static readonly string[] s_clientNames = ["WildStar64.exe", "WildStar32.exe"];

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
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
        // The program sits in the client's folder, so the client and config.json are found there whatever folder it was started from.
        string folder = AppContext.BaseDirectory;
        string? client = FindClient(folder);
        if (client is null)
        {
            Console.WriteLine($"Neither WildStar64.exe nor WildStar32.exe is in {folder}; copy this program into your client's folder.");
            return;
        }

        if (!File.Exists(Path.Combine(folder, JsonFile)))
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

            SaveConfiguration(folder, new ClientConfiguration
            {
                HostName = hostName,
                Language = language
            });
        }

        LaunchClient(client, folder, LoadConfiguration(folder));
    }

    /// <summary>
    /// The full path of the client in <paramref name="folder"/>: <c>WildStar64.exe</c>, else <c>WildStar32.exe</c>, else null.
    /// </summary>
    internal static string? FindClient(string folder)
    {
        foreach (string name in s_clientNames)
        {
            string path = Path.Combine(folder, name);
            if (File.Exists(path))
                return path;
        }

        return null;
    }

    /// <summary>
    /// Write <paramref name="config"/> to <c>config.json</c> in <paramref name="folder"/>.
    /// </summary>
    internal static void SaveConfiguration(string folder, ClientConfiguration config) =>
        File.WriteAllText(Path.Combine(folder, JsonFile), JsonConvert.SerializeObject(config));

    /// <summary>
    /// Read <c>config.json</c> from <paramref name="folder"/>, through <see cref="ReadConfiguration"/>.
    /// </summary>
    internal static ClientConfiguration LoadConfiguration(string folder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(folder)
            .AddJsonFile(JsonFile, optional: false)
            .Build();

        return ReadConfiguration(configuration);
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

    // The command line starts with /auth and carries no program name: the client reads its first token as /auth.
    private static void LaunchClient(string client, string folder, ClientConfiguration config)
    {
        var si = new STARTUPINFO();
        var pi = new PROCESS_INFORMATION();

        if (!CreateProcess(client,
            $"/auth {config.HostName} /authNc {config.HostName} /lang {config.Language} /patcher {config.HostName} /SettingsKey WildStar /realmDataCenterId 9",
            IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, folder, ref si, out pi))
        {
            Console.WriteLine($"Could not start {client}: Windows error {Marshal.GetLastWin32Error()}.");
        }
    }
}
