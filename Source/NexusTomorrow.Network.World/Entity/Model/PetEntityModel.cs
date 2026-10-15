using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Model;

[WireStruct]
public sealed partial class PetEntityModel : IEntityModel
{
    [UInt(18)] public uint CreatureId { get; set; }
    [UInt(32)] public uint OwnerId { get; set; }
    [UInt(15)] public ushort Unknown1 { get; set; }
    [WideString] public string Name { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 18u);
        writer.Write(OwnerId);
        writer.Write(Unknown1, 15u);
        writer.WriteStringWide(Name);
    }
}
