using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetVelocityDefaults)]
[WireStruct]
[UnionCase(EntityCommand.SetVelocityDefaults)]
public sealed partial class SetVelocityDefaultsCommand : IEntityCommandModel
{
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Blend);
    }
}
