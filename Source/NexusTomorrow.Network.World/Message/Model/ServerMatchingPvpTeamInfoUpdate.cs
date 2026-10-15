using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Send whenever the information for a team in a PvP match is updated during a match. This includes when a team first joins a match.
[Message(GameMessageOpcode.ServerMatchingPvpTeamInfoUpdate)]
[WireMessage(GameMessageOpcode.ServerMatchingPvpTeamInfoUpdate, Direction.ServerToClient)]
public sealed partial class ServerMatchingPvpTeamInfoUpdate : IWritable
{
    [WideString] public string Team1Name { get; set; }
    [WideString] public string Team2Name { get; set; }
    [UInt(32)] public uint Team1Rating { get; set; }
    [UInt(32)] public uint Team2Rating { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Team1Name);
        writer.WriteStringWide(Team2Name);
        writer.Write(Team1Rating);
        writer.Write(Team2Rating);
    }
}
