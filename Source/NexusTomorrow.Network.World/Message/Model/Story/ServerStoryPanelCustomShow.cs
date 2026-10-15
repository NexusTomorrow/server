using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story
{
    [Message(GameMessageOpcode.ServerStoryPanelCustomShow)]
    public class ServerStoryPanelCustomShow : IWritable
    {
        public StoryMessage StoryMessage { get; set; }
        public uint SoundContextEventId { get; set; }
        public StoryPanelType StoryPanelType { get; set; }
        public uint DurationMS { get; set; }
        public StoryPanelStyle StoryPanelStyle { get; set; }

        public void Write(GamePacketWriter writer)
        {
            StoryMessage.Write(writer);
            writer.Write(SoundContextEventId);
            writer.Write(StoryPanelType, 32u);
            writer.Write(DurationMS);
            writer.Write(StoryPanelStyle, 32u);
        }
    }
}
