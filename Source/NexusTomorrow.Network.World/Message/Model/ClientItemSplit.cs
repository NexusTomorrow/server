using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientItemSplit, Direction.ClientToServer)]
public sealed partial class ClientItemSplit
{
    [UInt(64)] public ulong Guid { get; private set; }
    public ItemLocation Location { get; } = new();
    [UInt(32)] public uint Count { get; private set; }
}
