using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventBombDropped)]
[WireMessage(GameMessageOpcode.ServerPublicEventBombDropped, Direction.ServerToClient)]
public sealed partial class ServerPublicEventBombDropped : IWritable
{
    [UInt(32)] public uint CarrierUnitId { get; set; }
    [UInt(1)] public bool Unknown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CarrierUnitId);
        writer.Write(Unknown);
    }
}
