using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[Message(GameMessageOpcode.ServerWarPartyMatchResults)]
[WireMessage(GameMessageOpcode.ServerWarPartyMatchResults, Direction.ServerToClient)]
public sealed partial class ServerWarPartyMatchResults : IWritable
{
    [UInt(32)] public uint Rating { get; set; }
    [UInt(32)] public uint RepairCost { get; set; }
    [UInt(32)] public uint DestroyedPlugs { get; set; }
    [UInt(32)] public uint WarCoinsEarned { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Rating);
        writer.Write(RepairCost);
        writer.Write(DestroyedPlugs);
        writer.Write(WarCoinsEarned);
    }
}
