using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchLeft)]
public class ServerMatchingMatchLeft : IWritable
{
    public Game.Static.Matching.MatchType Type { get; set; } // Not used by client, but server did fill this

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 5u);
    }
}
