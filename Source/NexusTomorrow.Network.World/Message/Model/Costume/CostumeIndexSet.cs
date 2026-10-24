using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Client0256, Direction.ClientToServer)]
public sealed partial class CostumeIndexSet
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
