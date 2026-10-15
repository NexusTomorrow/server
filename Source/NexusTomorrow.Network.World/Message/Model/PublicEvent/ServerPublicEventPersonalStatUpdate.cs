using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventPersonalStatUpdate)]
[WireMessage(GameMessageOpcode.ServerPublicEventPersonalStatUpdate, Direction.ServerToClient)]
public sealed partial class ServerPublicEventPersonalStatUpdate : IWritable
{
    [UInt(14)] public uint EventId { get; set; }
    [UInt(32)] public PublicEventStat StatType { get; set; }
    [UInt(32)] public int Value { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(EventId, 14u);
        writer.Write(StatType, 32u);
        writer.Write(Value);
    }
}
