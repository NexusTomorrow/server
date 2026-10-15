using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class WhoParameterRace : IWhoParameterData
{
    [UInt(8)] public Race RaceId { get; private set; }
    [Pad(6)] public Pad RaceIdHigh { get; }

    public void Read(GamePacketReader reader)
    {
        RaceId = reader.ReadEnum<Race>(14u);
    }
}
