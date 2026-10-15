using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.ServerInspectPlayerResponse, Direction.ServerToClient)]
public sealed partial class ServerInspectPlayerResponse
{
    [UInt(32)] public uint UnitId { get; set; }
    [Array(CountBits = 5)] public List<Item> Items { get; set; } = [];
}
