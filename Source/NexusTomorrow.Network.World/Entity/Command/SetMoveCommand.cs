using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetMove)]
[WireStruct]
[UnionCase(EntityCommand.SetMove)]
public sealed partial class SetMoveCommand : IEntityCommandModel
{
    [PackedFloat] public Vector3 Move { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Move = reader.ReadPackedVector3();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WritePackedVector3(Move);
        writer.Write(Blend);
    }
}
