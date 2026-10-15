using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class WarPartyBossToken
{
    [UInt(18)] public uint TokenItem2Id { get; set; }
    [UInt(32)] public uint Count { get; set; }
}
