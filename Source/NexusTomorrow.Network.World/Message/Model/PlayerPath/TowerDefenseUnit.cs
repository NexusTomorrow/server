using NexusTomorrow.Game.Static.PlayerPath;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireStruct]
public sealed partial class TowerDefenseUnit
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public TowerDefenseUnitType Type { get; set; }
}
