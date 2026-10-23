using NexusTomorrow.Gateway;
using NexusTomorrow.Maps;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Persistence;
using NexusTomorrow.Server;
using NexusTomorrow.Settings;

namespace NexusTomorrow.ProtocolReference.Tests;

public sealed class SettingsPageTests
{
    private static readonly PageStamp s_stamp = new("dev", "abc1234", "2026-10-23");
    private static readonly DocText s_docs = DocText.Load([typeof(SampleSettings).Assembly]);

    [Fact]
    public void TheSettingsPageMatchesTheHandWrittenPage()
    {
        const string expected = """
            ![reference, committed](https://img.shields.io/badge/reference-committed-2ea44f) ![verified 2026-10-23](https://img.shields.io/badge/verified-2026--10--23-6e7781) [![dev abc1234](https://img.shields.io/badge/dev-abc1234-6e7781)](https://github.com/NexusTomorrow/server/commit/abc1234)

            Every setting a Release build reads: its default, what the host checks at start-up, and what it sets. A setting that fails its check stops the server before it listens, naming the setting, never its value ([Host lifecycle](Host-lifecycle#settings)).

            A setting can also come from the environment, its name with `__` for each `:`, such as `Maps__TickMs`.

            > [!NOTE]
            > Generated from the settings classes at dev `abc1234` by `Tools/NexusTomorrow.ProtocolReference`. Don't edit it by hand: change the setting's documentation or its rule and regenerate.

            ## Sample

            [Map scheduler](Map-scheduler#knobs) and [Owners](Owners#knobs) explain these.

            | Setting | Default | Must be | Sets |
            |---|---|---|---|
            | **`Sample:ConnectionString`** | none | Set | The connection the sample opens; a secret, never logged |
            | **`Sample:Workers`** | the logical processors less two, at least 1 | At least 1 | How many workers run the samples; the default leaves two processors free |
            | **`Sample:Id`** | 1 | 1 to 16,383 | The sample's id |
            | **`Sample:TickMs`** | 25 ms | One of 25, 50, 125 or 250 | A sample tick in milliseconds |
            | **`Sample:BatchTicks`** | 5 | | How many ticks make one batch |

            `Sample:TickMs` times `Sample:BatchTicks` must be from 25 to 1,000 ms.

            ## Quiet

            [Command layer](Command-layer#knobs) explains these.

            | Setting | Default | Must be | Sets |
            |---|---|---|---|
            | **`Quiet:Enabled`** | on | | Whether the sample console runs |
            | **`Quiet:Wait`** | 30 s | | How long the sample waits |
            | **`Quiet:Folder`** | none | | Where the sample reads from |
            """;

        // Given out of order: the page orders its sections by the order each declares.
        string page = PageWriter.SettingsPage([SettingsSection.Of(typeof(QuietSettings)), SettingsSection.Of(typeof(SampleSettings))], s_docs, s_stamp);

        Assert.Equal(expected.ReplaceLineEndings("\n") + "\n", page);
    }

    [Fact]
    public void ASectionWithNoRuleShowsEmptyMustBeCells()
    {
        string[] blocks = PageWriter.SettingsPage([SettingsSection.Of(typeof(QuietSettings))], s_docs, s_stamp).Split("\n\n");

        Assert.Equal("## Quiet", blocks[4]);
        Assert.Equal(
            "| Setting | Default | Must be | Sets |\n|---|---|---|---|\n"
            + "| **`Quiet:Enabled`** | on | | Whether the sample console runs |\n"
            + "| **`Quiet:Wait`** | 30 s | | How long the sample waits |\n"
            + "| **`Quiet:Folder`** | none | | Where the sample reads from |\n",
            blocks[6]);
    }

    [Fact]
    public void TheServersSettingsPageOpensEachSectionWithItsPagesAndEndsItWithItsCrossFieldRule()
    {
        string[] blocks = PageWriter.SettingsPage(Registries.SettingsSections(), DocText.Load(Registries.SettingsAssemblies), s_stamp).Split("\n\n");

        (string Heading, string Pages, string? After)[] sections = [.. blocks
            .Select((block, index) => (Block: block, Index: index))
            .Where(item => item.Block.StartsWith("## ", StringComparison.Ordinal))
            .Select(item => (item.Block, blocks[item.Index + 1], item.Index + 3 < blocks.Length && !blocks[item.Index + 3].StartsWith("## ", StringComparison.Ordinal) ? blocks[item.Index + 3] : null))];

        (string, string, string?)[] expected =
        [
            ("## Gateway", "[Gateway](Gateway#knobs) explains these.", "`Gateway:Sts:Port`, `Gateway:Login:Port` and `Gateway:World:Port` must be three different ports."),
            ("## Account", "[Owners](Owners#knobs) explains these.", null),
            ("## Realm", "[Owners](Owners#knobs) explains these.", null),
            ("## Maps", "[Map scheduler](Map-scheduler#knobs) and [Owners](Owners#knobs) explain these.", "`Maps:TickMs` times `Maps:ReplicationEveryTicks` must be from 25 to 1,000 ms."),
            ("## Persistence", "[Persistence](Persistence#knobs) explains these.", "`Persistence:FirstRetryDelay` must not exceed `Persistence:MaxRetryDelay`."),
            ("## Cli", "[Command layer](Command-layer#knobs) explains these.", null),
            ("## Content", "[Owners](Owners#knobs) explains these.", null),
        ];
        Assert.Equal(expected, sections);
    }

    [Theory]
    [InlineData(typeof(GatewayOptions), "Sts:Port", "6600", "1 to 65,535")]
    [InlineData(typeof(GatewayOptions), "Login:BindAddress", "127.0.0.1", "An IP address")]
    [InlineData(typeof(GatewayOptions), "WorldPublicAddress", "127.0.0.1", "An IPv4 address")]
    [InlineData(typeof(GatewayOptions), "MaxClientFrameBytes", "65536 bytes", "64 to 1,048,576")]
    [InlineData(typeof(GatewayOptions), "PreAuthTimeout", "30 s", "Positive")]
    [InlineData(typeof(GatewayOptions), "DisconnectGrace", "5 s", "0 to 60 s")]
    [InlineData(typeof(GatewayOptions), "UnknownAccountSecret", "none", "Set: base64 of at least 32 bytes")]
    [InlineData(typeof(GatewayOptions), "DisabledOpcodes", "none", "")]
    [InlineData(typeof(RealmOptions), "Name", "NexusTomorrow", "Set")]
    [InlineData(typeof(RealmOptions), "StartLocations", "none", "")]
    [InlineData(typeof(RealmOptions), "StartLocations:<n>:WorldId", "0", "")]
    [InlineData(typeof(MapOptions), "PoolThreads", "the logical processors less three, at least 1", "At least 1")]
    [InlineData(typeof(MapOptions), "DedicatedThreads", "0", "Not negative")]
    [InlineData(typeof(MapOptions), "TickMs", "25 ms", "One of 25, 50, 125 or 250")]
    [InlineData(typeof(MapOptions), "SaveInterval", "60 s", "At least 1 s")]
    [InlineData(typeof(MapOptions), "NormalDrainBudget", "5 ms", "Positive")]
    [InlineData(typeof(PersistenceOptions), "ConnectionString", "none", "Set")]
    [InlineData(typeof(PersistenceOptions), "FirstRetryDelay", "100 ms", "Positive")]
    [InlineData(typeof(CliOptions), "Enabled", "on", "")]
    [InlineData(typeof(ContentOptions), "ClientDataPath", "none", "")]
    public void EachServerSettingShowsItsDefaultAndItsRulesAsTheConfigurationHoldsThem(Type options, string key, string expectedDefault, string expectedMustBe)
    {
        Setting setting = SettingsSection.Of(options).Find(key);

        Assert.Equal(expectedDefault, setting.DefaultText);
        Assert.Equal(expectedMustBe, setting.MustBe);
    }
}
