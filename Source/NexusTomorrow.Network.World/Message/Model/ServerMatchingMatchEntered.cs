using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchEntered)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchEntered, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchEntered : IWritable
{
    [UInt(32)] public uint MatchingGameMapId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MatchingGameMapId);
    }
}
