using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireStruct]
public sealed partial class GenericMapNode : IWritable
{
    [UInt(14)] public uint GenericMapModeId { get; set; }
    [UInt(1)] public bool Enabled { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GenericMapModeId, 14u);
        writer.Write(Enabled);
    }
}
