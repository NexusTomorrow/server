using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
public sealed partial class StoryMessage
{
    // One element of Actors: the 3-bit source type, then the actor it selects.
    [WireStruct]
    public sealed partial class ActorEntry
    {
        [UInt(3)] public StoryTextSourceType Type { get; set; }
        [Union(nameof(Type))] public Actor Actor { get; set; }
    }

    [UInt(32)] public uint MsgId { get; set; } // StoryPanelId for message 0x75A otherwise LocalizedTextId
    [UInt(32)] public uint RandomTextLineId { get; set; }
    [Array(CountBits = 8)] public List<ActorEntry> Actors { get; set; } = [];
}
