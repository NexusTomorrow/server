using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server0980, Direction.ServerToClient)]
public sealed partial class Server0980
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
