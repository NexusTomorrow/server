using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ClientResetInstances)]
[WireMessage(GameMessageOpcode.ClientResetInstances, Direction.ClientToServer)]
public sealed partial class ClientResetInstances : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
