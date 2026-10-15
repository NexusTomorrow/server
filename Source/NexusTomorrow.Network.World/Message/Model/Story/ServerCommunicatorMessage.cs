using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.ServerCommunicatorMessage, Direction.ServerToClient)]
public sealed partial class ServerCommunicatorMessage
{
    [UInt(15)] public ushort CommunicatorMessagesId { get; set; }

    // If false, this will make the message appear even if the player doesn't meet the
    // conditions for the message
    [UInt(1)] public bool CheckConditions { get; set; }
}
