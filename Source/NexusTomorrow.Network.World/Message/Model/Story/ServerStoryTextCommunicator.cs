using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.ServerStoryTextCommunicator, Direction.ServerToClient)]
public sealed partial class ServerStoryTextCommunicator
{
    public StoryMessage StoryMessage { get; set; }
    [UInt(18)] public uint Creature2Id { get; set; }
    [UInt(32)] public uint DurationMs { get; set; } = 10000;
    [UInt(2)] public CommunicatorPortraitPlacement PortraitPlacement { get; set; }
    [UInt(2)] public CommunicatorOverlay Overlay { get; set; }
    [UInt(3)] public CommunicatorBackground Background { get; set; }
}
