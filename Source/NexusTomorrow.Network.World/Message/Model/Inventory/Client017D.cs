using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client017D, Direction.ClientToServer)]
public sealed partial class Client017D
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    public Server0111Part2 Unknown1 { get; } = new();
    [UInt(32)] public uint Unknown2 { get; private set; }
}
