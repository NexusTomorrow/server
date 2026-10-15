using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.ClientICCommMessage, Direction.ClientToServer)]
public sealed partial class ClientICCommMessage
{
    [UInt(64)] public ulong IccommId { get; private set; }
    [UInt(32)] public uint MessageId { get; private set; }
    [NarrowString] public string Message { get; private set; }
    [WideString] public string RecipientName { get; private set; } // only sent for private messages
}
