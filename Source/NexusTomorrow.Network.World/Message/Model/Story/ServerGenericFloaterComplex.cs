using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// Appears over the player's unit
// In practice the only time this message was sent was to sent LocalizedTextId = 0x5F95C for the text 'Evade'
[Message(GameMessageOpcode.ServerGenericFloaterComplex)]
[WireMessage(GameMessageOpcode.ServerGenericFloaterComplex, Direction.ServerToClient)]
public sealed partial class ServerGenericFloaterComplex : IWritable
{
    [UInt(32)] public uint LocalisedTextId { get; set; }
    [UInt(32)] public uint RandomTextLineId { get; set; }
    [Array(CountBits = 8)] public List<StoryMessage> Messages { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LocalisedTextId);
        writer.Write(RandomTextLineId);
        writer.Write(Messages.Count, 8u);
        Messages.ForEach(message => message.Write(writer));
    }
}
