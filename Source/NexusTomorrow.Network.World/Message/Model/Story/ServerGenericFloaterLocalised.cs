using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// Appears over the player's unit
[WireMessage(GameMessageOpcode.ServerGenericFloaterLocalized, Direction.ServerToClient)]
public sealed partial class ServerGenericFloaterLocalised
{
    [UInt(32)] public uint LocalisedTextId { get; set; }
}
