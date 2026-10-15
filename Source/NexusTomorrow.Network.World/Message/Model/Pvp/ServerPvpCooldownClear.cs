using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Sets the players PVP cooldown to zero
[Message(GameMessageOpcode.ServerPvpCooldownClear)]
[WireMessage(GameMessageOpcode.ServerPvpCooldownClear, Direction.ServerToClient)]
public sealed partial class ServerPvpCooldownClear : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
