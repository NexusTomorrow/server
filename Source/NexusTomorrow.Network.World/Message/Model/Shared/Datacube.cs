using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Datacube : IWritable
{
    [UInt(14)] public ushort DatacubeId { get; set; }
    [UInt(32)] public uint Progress { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DatacubeId, 14u);
        writer.Write(Progress);
    }
}
