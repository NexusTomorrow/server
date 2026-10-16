using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(5)]
public sealed partial class Client0959Union0Arm05 : IClient0959Union0
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
