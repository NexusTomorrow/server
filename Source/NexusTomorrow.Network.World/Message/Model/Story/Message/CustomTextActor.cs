using NexusTomorrow.Game.Static.Story;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

public sealed class CustomTextActor : Actor
{
    public override StoryTextSourceType Type => StoryTextSourceType.CustomText;
    public string Text { get; set; }

    public override void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Text);
        base.Write(writer);
    }
}
