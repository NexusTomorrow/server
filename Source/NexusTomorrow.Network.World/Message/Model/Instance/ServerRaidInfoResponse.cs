using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.ServerRaidInfoResponse, Direction.ServerToClient)]
public sealed partial class ServerRaidInfoResponse
{
    [WireStruct]
    public sealed partial class RaidInfo
    {
        [UInt(64)] public ulong SavedInstanceId { get; set; }
        [UInt(15)] public ushort WorldId { get; set; }
        [UInt(64)] public ulong DateExpireUTC { get; set; } // Full date the lock resets
        [Float32] public float DaysUntilExpire { get; set; } // Relative time from now the lock resests
        [UInt(32)] public uint PrimeLevel { get; set; }
    }

    [Array(CountBits = 32)] private List<RaidInfo> Raids { get; set; } = [];
}
