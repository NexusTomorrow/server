using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Originally called Eldan Augmentations. See eldanAugmentation tbl for information
[WireMessage(GameMessageOpcode.ServerAmpList, Direction.ServerToClient)]
public sealed partial class ServerAmpList
{
    [UInt(3)] public byte SpecIndex { get; set; }
    [Array(CountBits = 7), UInt(16)] public List<ushort> Amps { get; set; } = []; // EldanAugmentationIds
}
