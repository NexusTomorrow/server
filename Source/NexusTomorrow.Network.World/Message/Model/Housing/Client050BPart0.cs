using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class Client050BPart0
{
    public Client04F5Part0 Unknown0 { get; internal set; } = new();
}
