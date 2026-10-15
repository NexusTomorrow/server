using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[Message(GameMessageOpcode.ServerDuelLeftArea)]
[WireMessage(GameMessageOpcode.ServerDuelLeftArea, Direction.ServerToClient)]
public sealed partial class ServerDuelLeftArea : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
