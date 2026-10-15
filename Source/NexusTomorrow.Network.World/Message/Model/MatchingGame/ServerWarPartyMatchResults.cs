using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame
{
    [Message(GameMessageOpcode.ServerWarPartyMatchResults)]
    public class ServerWarPartyMatchResults : IWritable
    {
        public uint Rating { get; set; }
        public uint RepairCost { get; set; }
        public uint DestroyedPlugs { get; set; }
        public uint WarCoinsEarned { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Rating);
            writer.Write(RepairCost);
            writer.Write(DestroyedPlugs);
            writer.Write(WarCoinsEarned);
        }
    }
}
