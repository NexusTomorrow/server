using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client0166, Direction.ClientToServer)]
public sealed partial class Client0166
{
    public Server0111Part2 Unknown0 { get; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
}
