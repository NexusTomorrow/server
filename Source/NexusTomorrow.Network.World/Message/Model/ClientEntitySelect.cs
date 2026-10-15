using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientEntitySelect, Direction.ClientToServer)]
public sealed partial class ClientEntitySelect
{
    [UInt(32)] public uint Guid { get; private set; }
}
