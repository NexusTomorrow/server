using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Game.Static.Entity.Movement.Command.Mode;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetMode)]
[WireStruct]
public sealed partial class SetModeCommand : IEntityCommandModel
{
    [UInt(32)] public ModeType Mode { get; set; }

    public void Read(GamePacketReader reader)
    {
        Mode = reader.ReadEnum<ModeType>(32);
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Mode, 32);
    }
}
