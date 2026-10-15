using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildPvpStatsUpdate)]
[WireMessage(GameMessageOpcode.ServerGuildPvpStatsUpdate, Direction.ServerToClient)]
public sealed partial class ServerGuildPvpStatsUpdate : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(32)] public uint Wins { get; set; }
    [UInt(32)] public uint Losses { get; set; }
    [UInt(32)] public uint Draws { get; set; }
    [UInt(32)] public uint Rating { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(Wins);
        writer.Write(Losses);
        writer.Write(Draws);
        writer.Write(Rating);
    }
}
