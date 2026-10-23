using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets group target markers: the client accepts it when the id matches one of its two groups, applies the marks to its first group,
/// and raises Group_SetMark for a mark only when that group is the current one and the TargetMarker table holds the marker.
/// </summary>
[WireMessage(GameMessageOpcode.Server0431, Direction.ServerToClient)]
public sealed partial class Server0431
{
    [WireStruct]
    public sealed partial class Server0431Part0
    {
        [UInt(32)] public uint Unknown0 { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
    }

    [UInt(64)] public ulong Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0431Part0> Unknown1 { get; } = [];
}
