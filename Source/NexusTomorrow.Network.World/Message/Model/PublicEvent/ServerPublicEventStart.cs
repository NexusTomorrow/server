using NexusTomorrow.Game.Static.PublicEvent;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[Message(GameMessageOpcode.ServerPublicEventStart)]
[WireMessage(GameMessageOpcode.ServerPublicEventStart, Direction.ServerToClient)]
public sealed partial class ServerPublicEventStart : IWritable
{
    [UInt(14)] public uint PublicEventId { get; set; }
    [UInt(14)] public PublicEventTeam PublicEventTeamId { get; set; }
    [UInt(32)] public uint ElapsedTimeMs { get; set; }
    [UInt(1)] public bool Busy { get; set; }
    [Array(CountBits = 32)] public List<PublicEventObjective> Objectives { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Locations { get; set; } = []; // array of worldLocation2Id
    [UInt(32)] public PublicEventRewardType RewardType { get; set; }
    [Pad(32)] public Pad RewardTypeHigh { get; }
    [FixedArray(3), UInt(32)] public uint[] RewardThreshold { get; set; } = new uint[3];
    [UInt(14)] public uint WorldSocketId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PublicEventId, 14);
        writer.Write(PublicEventTeamId, 14);
        writer.Write(ElapsedTimeMs);
        writer.Write(Busy);

        writer.Write(Objectives.Count);
        foreach (PublicEventObjective objective in Objectives)
            objective.Write(writer);

        writer.Write(Locations.Count);
        foreach (uint location in Locations)
            writer.Write(location);

        writer.Write(RewardType);

        foreach (uint threshold in RewardThreshold)
            writer.Write(threshold);

        writer.Write(WorldSocketId, 14u);
    }
}
