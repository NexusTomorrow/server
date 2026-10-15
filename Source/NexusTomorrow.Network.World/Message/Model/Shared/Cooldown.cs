using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Cooldown
{
    [UInt(3)] public byte Type { get; set; }
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(32)] public uint TypeId { get; set; }
    [UInt(32)] public uint TimeRemaining { get; set; }
}
