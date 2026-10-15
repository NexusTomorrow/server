using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Class)]
public sealed partial class WhoParameterClass : IWhoParameterData
{
    [UInt(8)] public Class ClassId { get; private set; }
    [Pad(6)] public Pad ClassIdHigh { get; }
}
