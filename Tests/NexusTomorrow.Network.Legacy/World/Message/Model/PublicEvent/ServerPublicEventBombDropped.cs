using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventBombDropped)]
public class ServerPublicEventBombDropped : IWritable
{
    public uint CarrierUnitId { get; set; }
    public bool Unknown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CarrierUnitId);
        writer.Write(Unknown);
    }
}
