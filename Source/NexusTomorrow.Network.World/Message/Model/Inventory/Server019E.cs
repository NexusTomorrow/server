using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server019E, Direction.ServerToClient)]
public sealed partial class Server019E
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
