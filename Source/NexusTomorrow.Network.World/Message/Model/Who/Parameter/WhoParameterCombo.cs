using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Combo)]
public sealed partial class WhoParameterCombo : IWhoParameterData
{
    [WideString] public string SearchString { get; private set; }
    [UInt(8)] public Race RaceId { get; private set; }
    [Pad(6)] public Pad RaceIdHigh { get; }
    [UInt(3)] public Path PathId { get; private set; }
    [UInt(8)] public Class ClassId { get; private set; }
    [Pad(6)] public Pad ClassIdHigh { get; }
    [UInt(15)] public ushort WorldZoneId { get; private set; }
    [UInt(14)] public ushort WorldZoneId2 { get; private set; } // usually a duplicate of WorldZoneId, not sure why this is only 14 bits

    public void Read(GamePacketReader reader)
    {
        SearchString = reader.ReadWideString();
        RaceId = reader.ReadEnum<Race>(14u);
        PathId = reader.ReadEnum<Path>(3u);
        ClassId = reader.ReadEnum<Class>(14u);
        WorldZoneId = reader.ReadUShort(15u);
        WorldZoneId2 = reader.ReadUShort(14u);
    }
}
