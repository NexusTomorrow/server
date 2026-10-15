using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NexusTomorrow.GameTable.Configuration.Model;

namespace NexusTomorrow.GameTable.Tests;

public class GameTableConfigTests
{
    /// <summary>
    /// Register the GameTable options from the supplied keys and resolve them, which runs their validation.
    /// </summary>
    private static GameTableConfig Resolve(Dictionary<string, string?> values)
    {
        IConfigurationSection section = new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build()
            .GetSection("GameTable");

        var services = new ServiceCollection();
        services.AddGameTable(section);

        using ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IOptions<GameTableConfig>>().Value;
    }

    [Fact]
    public void MissingGameTablePathFailsValidation()
    {
        Assert.Throws<OptionsValidationException>(() => Resolve(new() { ["GameTable:Cache:UseCache"] = "false" }));
    }

    [Fact]
    public void EmptyGameTablePathFailsValidation()
    {
        Assert.Throws<OptionsValidationException>(() => Resolve(new() { ["GameTable:GameTablePath"] = "" }));
    }

    [Fact]
    public void BoundGameTablePathPassesValidation()
    {
        GameTableConfig config = Resolve(new() { ["GameTable:GameTablePath"] = "tbl" });
        Assert.Equal("tbl", config.GameTablePath);
    }
}
