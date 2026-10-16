using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096F, Direction.ServerToClient)]
public sealed partial class Server096F
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
