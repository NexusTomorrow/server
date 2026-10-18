using Microsoft.Extensions.Configuration;
using NexusTomorrow.ClientConnector.Configuration;

namespace NexusTomorrow.ClientConnector.Tests;

public class ProgramTests
{
    /// <summary>
    /// Build a configuration holding only the supplied keys, as a config.json with those keys would.
    /// </summary>
    private static IConfiguration Configuration(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    [Fact]
    public void ReadConfigurationThrowsWhenLanguageIsMissing()
    {
        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => Program.ReadConfiguration(Configuration(new() { ["HostName"] = "localhost" })));
        Assert.Contains("Language", exception.Message);
    }

    [Fact]
    public void ReadConfigurationThrowsWhenHostNameIsEmpty()
    {
        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => Program.ReadConfiguration(Configuration(new() { ["HostName"] = "", ["Language"] = "en" })));
        Assert.Contains("HostName", exception.Message);
    }

    [Fact]
    public void ReadConfigurationReturnsBothKeys()
    {
        ClientConfiguration config = Program.ReadConfiguration(Configuration(new() { ["HostName"] = "localhost", ["Language"] = "en" }));
        Assert.Equal("localhost", config.HostName);
        Assert.Equal("en", config.Language);
    }

    [Fact]
    public void TheConfigurationIsSavedToAndLoadedFromTheGivenFolderNotTheCurrentOne()
    {
        string folder = Directory.CreateTempSubdirectory().FullName;
        try
        {
            Assert.NotEqual(Path.TrimEndingDirectorySeparator(folder), Path.TrimEndingDirectorySeparator(Environment.CurrentDirectory));
            Program.SaveConfiguration(folder, new ClientConfiguration { HostName = "example.org", Language = "de" });
            Assert.True(File.Exists(Path.Combine(folder, "config.json")));

            ClientConfiguration config = Program.LoadConfiguration(folder);
            Assert.Equal("example.org", config.HostName);
            Assert.Equal("de", config.Language);
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    [Theory]
    [InlineData(new[] { "WildStar64.exe", "WildStar32.exe" }, "WildStar64.exe")]
    [InlineData(new[] { "WildStar32.exe" }, "WildStar32.exe")]
    [InlineData(new string[0], null)]
    public void FindClientPrefersTheSixtyFourBitClientInTheGivenFolder(string[] present, string? expected)
    {
        string folder = Directory.CreateTempSubdirectory().FullName;
        try
        {
            foreach (string name in present)
            {
                File.WriteAllBytes(Path.Combine(folder, name), []);
            }

            Assert.Equal(expected is null ? null : Path.Combine(folder, expected), Program.FindClient(folder));
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }
}
