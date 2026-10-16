using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.Client0267, Direction.ClientToServer)]
public sealed partial class Client0267
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
    [UInt(64)] public ulong Unknown1 { get; private set; }
}
