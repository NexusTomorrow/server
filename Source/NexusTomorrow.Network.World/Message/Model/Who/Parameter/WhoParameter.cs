using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class WhoParameter : IReadable
{
    [UInt(4)] public WhoParameterType Type { get; private set; }
    [Union(nameof(Type))] public IWhoParameterData Data { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<WhoParameterType>(4u);

        Data = Type switch
        {
            WhoParameterType.Level => new WhoParameterLevel(),
            WhoParameterType.Race => new WhoParameterRace(),
            WhoParameterType.Path => new WhoParameterPath(),
            WhoParameterType.Class => new WhoParameterClass(),
            WhoParameterType.Zone => new WhoParameterZone(),
            WhoParameterType.Guild => new WhoParameterGuild(),
            WhoParameterType.Player => new WhoParameterPlayer(),
            WhoParameterType.Combo => new WhoParameterCombo(),
            WhoParameterType.Faction => new WhoParameterFaction(),
            _ => throw new NotImplementedException()
        };
        Data.Read(reader);
    }
}
