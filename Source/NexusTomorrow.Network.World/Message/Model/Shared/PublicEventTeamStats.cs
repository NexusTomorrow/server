using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PublicEventTeamStats
{
    [UInt(14)] public PublicEventTeam TeamId { get; set; }
    public PublicEventStats Stats { get; set; }
}
