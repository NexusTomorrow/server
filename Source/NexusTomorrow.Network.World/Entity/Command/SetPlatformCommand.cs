using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPlatform)]
[WireStruct]
[UnionCase(EntityCommand.SetPlatform)]
public sealed partial class SetPlatformCommand : IEntityCommandModel
{
    [UInt(32)] public uint UnitId { get; set; }

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
    }
}
