using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class GuildEventLog
{
    public Identity GuildIdentity { get; set; }
    [UInt(64)] public ulong EventLogId { get; set; }
    [UInt(17)] public GuildEventType EventType { get; set; }
    [UInt(64)] public ulong CharacterId { get; set; }
    [UInt(64)] public ulong Item2Id { get; set; }
    [UInt(64)] public ulong Amount { get; set; } // Can be money amount or item count for item operations
    [UInt(64)] public ulong Unknown { get; set; }
    [Float32] public float DaysAgoOccured { get; set; }
}
