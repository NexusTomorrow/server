using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server097B, Direction.ServerToClient)]
public sealed partial class Server097B
{
    [UInt(6)] public byte Unknown0 { get; set; }
}
