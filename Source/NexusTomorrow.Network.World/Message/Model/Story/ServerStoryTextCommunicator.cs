using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[Message(GameMessageOpcode.ServerStoryTextCommunicator)]
[WireMessage(GameMessageOpcode.ServerStoryTextCommunicator, Direction.ServerToClient)]
public sealed partial class ServerStoryTextCommunicator : IWritable
{
    public StoryMessage StoryMessage { get; set; }
    [UInt(18)] public uint Creature2Id { get; set; }
    [UInt(32)] public uint DurationMs { get; set; } = 10000;
    [UInt(2)] public CommunicatorPortraitPlacement PortraitPlacement { get; set; }
    [UInt(2)] public CommunicatorOverlay Overlay { get; set; }
    [UInt(3)] public CommunicatorBackground Background { get; set; }

    public void Write(GamePacketWriter writer)
    {
        StoryMessage.Write(writer);
        writer.Write(Creature2Id, 18u);
        writer.Write(DurationMs);
        writer.Write(PortraitPlacement, 2u);
        writer.Write(Overlay, 2u);
        writer.Write(Background, 3u);
    }
}
