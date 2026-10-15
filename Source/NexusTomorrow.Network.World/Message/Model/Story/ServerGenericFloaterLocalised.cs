using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// Appears over the player's unit
[Message(GameMessageOpcode.ServerGenericFloaterLocalized)]
[WireMessage(GameMessageOpcode.ServerGenericFloaterLocalized, Direction.ServerToClient)]
public sealed partial class ServerGenericFloaterLocalised : IWritable
{
    [UInt(32)] public uint LocalisedTextId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LocalisedTextId);
    }
}
