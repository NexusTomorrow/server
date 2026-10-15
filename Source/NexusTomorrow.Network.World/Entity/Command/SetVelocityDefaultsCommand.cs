using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetVelocityDefaults)]
public class SetVelocityDefaultsCommand : IEntityCommandModel
{
    public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Blend);
    }
}
