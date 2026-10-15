using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// Appears over the player's unit
[Message(GameMessageOpcode.ServerGenericFloaterLocalized)]
public class ServerGenericFloaterLocalised : IWritable
{
    public uint LocalisedTextId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LocalisedTextId);
    }
}
