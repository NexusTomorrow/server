using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic
{
    [Message(GameMessageOpcode.ServerCinematicStoryPanelDialogHide)]
    public class ServerCinematicStoryPanelDialogHide : IWritable
    {
        public uint Delay { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Delay);
        }
    }
}
