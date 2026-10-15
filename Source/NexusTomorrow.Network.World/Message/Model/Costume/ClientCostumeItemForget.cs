using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ClientCostumeItemForget, Direction.ClientToServer)]
public sealed partial class ClientCostumeItemForget
{
    [UInt(18)] public uint Item2Id { get; private set; }
}
