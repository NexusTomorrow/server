using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0788, Direction.ClientToServer)]
public sealed partial class Client0788
{
    public Client00B5Part0 Unknown0 { get; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
    [UInt(1)] public bool Unknown2 { get; private set; }
}
