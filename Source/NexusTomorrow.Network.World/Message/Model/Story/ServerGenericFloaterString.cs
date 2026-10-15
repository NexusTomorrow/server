using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story
{
    // Appears over the player's unit
    [Message(GameMessageOpcode.ServerGenericFloaterString)]
    public class ServerGenericFloaterString : IWritable
    {
        public string Text { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteStringWide(Text);
        }
    }
}
