using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class ScannerUnitEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(32)] public uint OwnerId { get; set; }
    [WideString] public string Name { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(OwnerId);
        writer.WriteStringWide(Name);
    }
}
