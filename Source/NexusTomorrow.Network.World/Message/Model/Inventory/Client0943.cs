using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client0943, Direction.ClientToServer)]
public sealed partial class Client0943
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    public Server0111Part2 Unknown1 { get; } = new();
    [UInt(32)] public uint Unknown2 { get; private set; }
    public Server0111Part2 Unknown3 { get; } = new();
    public Client0096Part0 Unknown4 { get; } = new Client0096Part0();
}
