using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Zone)]
public sealed partial class WhoParameterZone : IWhoParameterData
{
    [UInt(15)] public ushort WorldZoneId { get; private set; }
}
