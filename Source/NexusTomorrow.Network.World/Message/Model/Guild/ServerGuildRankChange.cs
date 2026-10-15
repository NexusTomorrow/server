using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildRankChange, Direction.ServerToClient)]
public sealed partial class ServerGuildRankChange
{
    public Identity GuildIdentity { get; set; }
    [FixedArray(10)] public GuildRank[] Ranks { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];
}
