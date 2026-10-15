using Microsoft.Extensions.Configuration;
using NexusTomorrow.ClientConnector.Configuration;

namespace NexusTomorrow.ClientConnector.Tests
{
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
    }
}
