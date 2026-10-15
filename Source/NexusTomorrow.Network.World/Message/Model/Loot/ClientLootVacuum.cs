using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

// Usable when the player has been sent with ServerLootNotify (0x07F2)
[Message(GameMessageOpcode.ClientLootVacuum)]
[WireMessage(GameMessageOpcode.ClientLootVacuum, Direction.ClientToServer)]
public sealed partial class ClientLootVacuum : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
