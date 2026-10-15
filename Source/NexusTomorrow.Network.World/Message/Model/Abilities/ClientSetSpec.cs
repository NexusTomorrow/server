using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ClientSetSpec, Direction.ClientToServer)]
public sealed partial class ClientSetSpec
{
    [UInt(3)] public byte SpecIndex { get; private set; }
}
