using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class WhoParameter
{
    [UInt(4)] public WhoParameterType Type { get; private set; }
    [Union(nameof(Type))] public IWhoParameterData Data { get; private set; }
}
