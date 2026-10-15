using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchLeft)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchLeft, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchLeft : IWritable
{
    [UInt(5)] public Game.Static.Matching.MatchType Type { get; set; } // Not used by client, but server did fill this

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 5u);
    }
}
