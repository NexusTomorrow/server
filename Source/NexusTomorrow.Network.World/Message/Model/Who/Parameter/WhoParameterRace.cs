using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Race)]
public sealed partial class WhoParameterRace : IWhoParameterData
{
    [UInt(8)] public Race RaceId { get; private set; }
    [Pad(6)] public Pad RaceIdHigh { get; }
}
