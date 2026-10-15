using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    // TODO:: confirm
    [Message(GameMessageOpcode.ServerCinematicAdvanceToEnding)] 
    public class ServerCinematicAdvanceToEnding : IWritable
    {
        public byte Unused { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unused);
        }
    }
}
