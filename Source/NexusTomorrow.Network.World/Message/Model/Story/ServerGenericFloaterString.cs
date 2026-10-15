using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// Appears over the player's unit
[Message(GameMessageOpcode.ServerGenericFloaterString)]
[WireMessage(GameMessageOpcode.ServerGenericFloaterString, Direction.ServerToClient)]
public sealed partial class ServerGenericFloaterString : IWritable
{
    [WideString] public string Text { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Text);
    }
}
