using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

[Message(GameMessageOpcode.ServerReputationUpdate)]
[WireMessage(GameMessageOpcode.ServerReputationUpdate, Direction.ServerToClient)]
public sealed partial class ServerReputationUpdate : IWritable
{
    [UInt(14)] public Faction FactionId { get; set; }
    [Float32] public float Value { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(FactionId, 14u);
        writer.Write(Value);
    }
}
