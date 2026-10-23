using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Maps.Worlds;
using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Maps.Tests.Worlds;

public sealed class WorldKindsTests
{
    [Theory]
    [InlineData(0u, 0u, WorldKind.OpenWorld)]
    [InlineData(0u, 1u, WorldKind.Held)]
    [InlineData(0u, 5u, WorldKind.Held)]
    [InlineData(0u, 2u, WorldKind.OpenWorld)]
    [InlineData(1u, 0u, WorldKind.Instanced)]
    [InlineData(11u, 0u, WorldKind.Instanced)]
    public void AWorldRowIsOpenWorldHeldOrInstanced(uint type, uint flags, WorldKind expected) =>
        Assert.Equal(expected, WorldKinds.Classify(type, flags));

    [Fact]
    public void WithNoClientDataEveryWorldIsUnknown()
    {
        var catalog = new TableWorldCatalog(Options.Create(new ContentOptions()), NullLogger<TableWorldCatalog>.Instance);

        Assert.Equal(WorldKind.Unknown, catalog.KindOf(1));
    }

    [Fact]
    public void AFixedCatalogKnowsOnlyItsWorlds()
    {
        var catalog = new FixedWorldCatalog(
            [new KeyValuePair<ushort, WorldKind>(1, WorldKind.OpenWorld), new KeyValuePair<ushort, WorldKind>(2, WorldKind.Held)]);

        Assert.Equal(WorldKind.OpenWorld, catalog.KindOf(1));
        Assert.Equal(WorldKind.Held, catalog.KindOf(2));
        Assert.Equal(WorldKind.Unknown, catalog.KindOf(3));
    }
}
