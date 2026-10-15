using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventStats
{
    [UInt(32)] public uint Mask { get; set; } // 32 flags, the first in the least significant bit
    [Array(CountBits = 5), UInt(32)] public List<uint> Values { get; set; } = [];
}
