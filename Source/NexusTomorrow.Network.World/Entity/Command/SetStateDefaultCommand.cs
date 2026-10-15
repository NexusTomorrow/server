using NexusTomorrow.Game.Static.Entity.Movement.Command;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetStateDefault)]
public class SetStateDefaultCommand : IEntityCommandModel
{
    public bool Strafe { get; set; }

    public void Read(GamePacketReader reader)
    {
        Strafe = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Strafe);
    }
}
