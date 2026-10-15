using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// Appears over the player's unit
[WireMessage(GameMessageOpcode.ServerGenericFloaterString, Direction.ServerToClient)]
public sealed partial class ServerGenericFloaterString
{
    [WideString] public string Text { get; set; }
}
