using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04C5, Direction.ServerToClient)]
public sealed partial class GuildRanksChanged
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [FixedArray(10)] public GuildRank[] Unknown1 { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];
}
