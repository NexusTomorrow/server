using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetTime)]
[WireStruct]
public sealed partial class SetTimeCommand : IEntityCommandModel
{
    [UInt(32)] public uint Time { get; set; }

    public void Read(GamePacketReader reader)
    {
        Time = reader.ReadUInt();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Time);
    }
}
