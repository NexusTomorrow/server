using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchJoined)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchJoined, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchJoined : IWritable
{
    [UInt(14)] public uint MatchingGameMapId { get; set; } // Id of map the player has just joined

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MatchingGameMapId, 0xEu);
    }
}
