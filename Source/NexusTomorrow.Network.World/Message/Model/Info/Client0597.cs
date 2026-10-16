using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Client0597, Direction.ClientToServer)]
public sealed partial class Client0597
{
    [UInt(4)] public byte Unknown0 { get; private set; }
    public Client00B5Part0 Unknown1 { get; } = new();
}
