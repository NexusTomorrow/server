using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetScale)]
[WireStruct]
[UnionCase(EntityCommand.SetScale)]
public sealed partial class SetScaleCommand : IEntityCommandModel
{
    [PackedFloat] public float Scale { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Scale = reader.ReadPackedFloat();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WritePackedFloat(Scale);
        writer.Write(Blend);
    }
}
