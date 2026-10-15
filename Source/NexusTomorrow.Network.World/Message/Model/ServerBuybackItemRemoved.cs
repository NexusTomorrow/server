using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerBuybackItemRemoved, Direction.ServerToClient)]
public sealed partial class ServerBuybackItemRemoved
{
    [UInt(32)] public uint UniqueId { get; set; }
}
