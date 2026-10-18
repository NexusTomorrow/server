using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server0566, Direction.ServerToClient)]
public sealed partial class Server0566
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> Unknown0 { get; set; } = [];
}
