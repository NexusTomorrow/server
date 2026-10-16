using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0525, Direction.ClientToServer)]
public sealed partial class Client0525
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
    [UInt(2)] public byte Unknown1 { get; private set; }
}
