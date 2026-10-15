using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.ServerPublicEventStart, Direction.ServerToClient)]
public sealed partial class ServerPublicEventStart
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [UInt(14)] public PublicEventTeam PublicEventTeamId { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }
    [UInt(1)] public bool Busy { get; set; }
    [Array(CountBits = 32)] public List<PublicEventObjective> Objectives { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Locations { get; set; } = []; // array of worldLocation2Id
    [UInt(32)] public PublicEventRewardType RewardType { get; set; }
    [FixedArray(3), UInt(32)] public uint[] RewardThreshold { get; set; } = new uint[3];
    [UInt(14)] public uint WorldSocketId { get; set; }
}
