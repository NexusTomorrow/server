using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Originally called Eldan Augmentations. See eldanAugmentation tbl for information
// One count, then every SpecIndex, then every Result: element i of each list is one AMP's result.
[WireMessage(GameMessageOpcode.ServerAmpRespecResult, Direction.ServerToClient)]
public sealed partial class ServerAmpRespecResult
{
    [UInt(7), Count] public byte ResultCount { get; set; }
    [Array(CountFrom = nameof(ResultCount)), UInt(3)] public List<ushort> SpecIndexes { get; set; } = [];
    [Array(CountFrom = nameof(ResultCount)), UInt(3)] public List<LimitedActionSetResult> Results { get; set; } = [];
}
