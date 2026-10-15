using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class WhoParameterZone : IWhoParameterData
{
    [UInt(15)] public ushort WorldZoneId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        WorldZoneId = reader.ReadUShort(15u);
    }
}
