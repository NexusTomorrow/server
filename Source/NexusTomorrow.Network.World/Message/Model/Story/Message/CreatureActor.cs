using NexusTomorrow.Game.Static.Story;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

public sealed class CreatureActor : Actor
{
    public override StoryTextSourceType Type => StoryTextSourceType.Creature;
    public uint Creature2Id { get; set; }

    public override void Write(GamePacketWriter writer)
    {
        writer.Write(Creature2Id, 18u);
        base.Write(writer);
    }
}
