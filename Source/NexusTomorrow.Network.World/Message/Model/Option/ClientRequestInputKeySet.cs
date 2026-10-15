using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.ClientRequestInputKeySet, Direction.ClientToServer)]
public sealed partial class ClientRequestInputKeySet
{
    [UInt(64)] public ulong CharacterId { get; private set; }
}
