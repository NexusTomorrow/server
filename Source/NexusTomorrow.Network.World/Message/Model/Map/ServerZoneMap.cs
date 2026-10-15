using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[Message(GameMessageOpcode.ServerZoneMap)]
[WireMessage(GameMessageOpcode.ServerZoneMap, Direction.ServerToClient)]
public sealed partial class ServerZoneMap : IWritable
{
    [UInt(32)] public uint ZoneMapId { get; set; }
    [NotOnWire] public uint Count { get; set; }
    [Blob(32)] public byte[] ZoneMapBits { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ZoneMapId);
        writer.Write(Count / 8);

        writer.WriteBytes(ZoneMapBits);
    }
}
