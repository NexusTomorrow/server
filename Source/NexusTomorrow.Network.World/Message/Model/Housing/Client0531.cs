using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0531, Direction.ClientToServer)]
public sealed partial class Client0531
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    public Client00B5Part0 Unknown1 { get; } = new();
    [WideString] public string Unknown2 { get; private set; }
    public Client00B5Part0 Unknown3 { get; } = new();
    [WideString] public string Unknown4 { get; private set; }
}
