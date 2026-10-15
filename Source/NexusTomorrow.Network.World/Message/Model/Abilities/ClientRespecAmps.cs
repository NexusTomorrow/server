using NexusTomorrow.Game.Static.Abilities;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ClientRespecAmps, Direction.ClientToServer)]
public sealed partial class ClientRespecAmps
{
    [UInt(3)] public byte SpecIndex { get; private set; }
    [UInt(3)] public AmpRespecType RespecType { get; private set; }
    [UInt(32)] public uint Value { get; private set; } // Is EldanAugmentationId for RespecType = Single
}
