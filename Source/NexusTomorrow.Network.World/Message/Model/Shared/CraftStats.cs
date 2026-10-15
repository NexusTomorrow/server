using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class CraftStats
{
    [FixedArray(5), UInt(8)] public Property[] StatType { get; set; } = new Property[5];
    [Pad(8)] public Pad Reserved { get; }
    [UInt(8)] public byte Unknown { get; set; }
    [UInt(8)] public byte ApSpSplit { get; set; } // Attack Power and Support Power split
}
