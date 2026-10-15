using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingCommit)]
[WireMessage(GameMessageOpcode.ClientP2PTradingCommit, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingCommit : IReadable
{
    // Zero byte message
    //
    // This message is sent when the player commits to the trade.
    // It is also used to cancel a trade however the UI clears the money and items first.
    // According to comments in the Carbine UI files, this is because the message was meant to
    // have an option to cancel which was never fixed.
    public void Read(GamePacketReader reader)
    {
    }
}
