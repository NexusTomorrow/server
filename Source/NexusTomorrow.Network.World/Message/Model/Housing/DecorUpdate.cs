using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class DecorUpdate
{
    public DecorInfo DecorInfo { get; internal set; } = new();
}
