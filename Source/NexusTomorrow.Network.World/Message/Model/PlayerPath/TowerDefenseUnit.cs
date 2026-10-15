using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireStruct]
public sealed partial class TowerDefenseUnit : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public TowerDefenseUnitType Type { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Type, 32u);
    }
}
