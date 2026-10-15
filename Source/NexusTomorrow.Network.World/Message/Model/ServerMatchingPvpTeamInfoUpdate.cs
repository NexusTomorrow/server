using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Send whenever the information for a team in a PvP match is updated during a match. This includes when a team first joins a match.
[WireMessage(GameMessageOpcode.ServerMatchingPvpTeamInfoUpdate, Direction.ServerToClient)]
public sealed partial class ServerMatchingPvpTeamInfoUpdate
{
    [WideString] public string Team1Name { get; set; }
    [WideString] public string Team2Name { get; set; }
    [UInt(32)] public uint Team1Rating { get; set; }
    [UInt(32)] public uint Team2Rating { get; set; }
}
