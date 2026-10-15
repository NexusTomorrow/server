using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message
{
    public abstract class Actor : IWritable
    {
        public virtual StoryTextSourceType Type { get; }
        public uint TokenReplacementValue { get; set; }
        public string TokenName { get; set; } // used to specify a custom token name in a localized string i.e. $(direction) $(enemy_fac) $(resource_type)

        public virtual void Write(GamePacketWriter writer)
        {
            writer.Write(TokenReplacementValue);
            writer.WriteString(TokenName);
        }
    }
}
