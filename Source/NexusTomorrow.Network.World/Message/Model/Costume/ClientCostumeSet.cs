using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ClientCostumeSet, Direction.ClientToServer)]
public sealed partial class ClientCostumeSet
{
    [UInt(32)] public int Index { get; private set; }
}
