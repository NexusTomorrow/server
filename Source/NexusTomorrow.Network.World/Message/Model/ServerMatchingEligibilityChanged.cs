using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingEligibilityChanged)]
    public class ServerMatchingEligibilityChanged : IWritable
    {
        public uint MatchingEligibilityFlags { get; set; } // Checked against tbl matchingMapPrequisite->matchingEligibilityFlagEnum

        public void Write(GamePacketWriter writer)
        {
            writer.Write(MatchingEligibilityFlags);
        }
    }
}
