using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildRankChange)]
[WireMessage(GameMessageOpcode.ServerGuildRankChange, Direction.ServerToClient)]
public sealed partial class ServerGuildRankChange : IWritable
{
    public Identity GuildIdentity { get; set; }
    [FixedArray(10)] public GuildRank[] Ranks { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        foreach (GuildRank rank in Ranks)
            rank.Write(writer);
    }
}
