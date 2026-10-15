using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetRotation)]
[WireStruct]
public sealed partial class SetRotationCommand : IEntityCommandModel
{
    [Float32] public Vector3 Rotation { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Rotation = reader.ReadVector3();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteVector3(Rotation);
        writer.Write(Blend);
    }
}
