using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

[Message(GameMessageOpcode.ServerReputationUpdate)]
public class ServerReputationUpdate : IWritable
{
    public Faction FactionId { get; set; }
    public float Value { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FactionId, 14u);
        writer.Write(Value);
    }
}
