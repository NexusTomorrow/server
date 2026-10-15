using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Path)]
public sealed partial class WhoParameterPath : IWhoParameterData
{
    [UInt(3)] public Path PathId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PathId = reader.ReadEnum<Path>(3u);
    }
}
