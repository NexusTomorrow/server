using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0428, Direction.ClientToServer)]
public sealed partial class Client0428
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    public Client00B5Part0 Unknown1 { get; set; } = new();
}
