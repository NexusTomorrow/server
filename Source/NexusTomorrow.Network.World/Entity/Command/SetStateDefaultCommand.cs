using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetStateDefault)]
[WireStruct]
[UnionCase(EntityCommand.SetStateDefault)]
public sealed partial class SetStateDefaultCommand : IEntityCommandModel
{
    [UInt(1)] public bool Strafe { get; set; }

    public void Read(GamePacketReader reader)
    {
        Strafe = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Strafe);
    }
}
