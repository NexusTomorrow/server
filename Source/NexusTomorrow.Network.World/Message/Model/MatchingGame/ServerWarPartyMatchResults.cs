using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.ServerWarPartyMatchResults, Direction.ServerToClient)]
public sealed partial class ServerWarPartyMatchResults
{
    [UInt(32)] public uint Rating { get; set; }
    [UInt(32)] public uint RepairCost { get; set; }
    [UInt(32)] public uint DestroyedPlugs { get; set; }
    [UInt(32)] public uint WarCoinsEarned { get; set; }
}
