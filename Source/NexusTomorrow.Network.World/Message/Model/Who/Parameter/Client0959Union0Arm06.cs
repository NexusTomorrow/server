using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(6)]
public sealed partial class Client0959Union0Arm06 : IClient0959Union0
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
