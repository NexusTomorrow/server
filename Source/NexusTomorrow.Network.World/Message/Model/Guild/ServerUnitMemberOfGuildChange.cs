using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// <summary> Sends all of the GuildIds to which a unit belongs.
// Circles, Warparties, Arena teams, and Communities all work the same on the client/server as normal Guilds so this
// list will contain any and all of the GuildIds to which the unit belongs. </summary>
[Message(GameMessageOpcode.ServerUnitMemberOfGuildChange)]
[WireMessage(GameMessageOpcode.ServerUnitMemberOfGuildChange, Direction.ServerToClient)]
public sealed partial class ServerUnitMemberOfGuildChange : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 32), UInt(64)] public List<ulong> GuildIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(GuildIds.Count);
        foreach (ulong guildId in GuildIds)
        {
            writer.Write(guildId);
        }
    }
}
