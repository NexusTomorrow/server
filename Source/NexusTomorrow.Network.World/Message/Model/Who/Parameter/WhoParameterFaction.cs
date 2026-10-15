using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Faction)]
public sealed partial class WhoParameterFaction : IWhoParameterData
{
    [UInt(14)] public Faction Faction2Id { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Faction2Id = reader.ReadEnum<Faction>(14u);
    }
}
