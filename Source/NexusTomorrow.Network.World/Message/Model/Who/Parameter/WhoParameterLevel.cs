using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Level)]
public sealed partial class WhoParameterLevel : IWhoParameterData
{
    [UInt(32)] public uint BottomLevel { get; private set; }
    [UInt(32)] public uint TopLevel { get; private set; }
}
