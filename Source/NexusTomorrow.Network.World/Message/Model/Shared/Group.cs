using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Group : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(32)] public GroupFlags Flags { get; set; }
    [UInt(32), Count] public uint MemberInfosCount { get; set; }
    [UInt(32)] public uint MaxGroupSize { get; set; }

    [UInt(3)] public LootRule LootRule { get; set; }
    [UInt(3)] public LootRule LootRuleThreshold { get; set; }
    [UInt(4)] public LootThreshold LootThreshold { get; set; }
    [UInt(2)] public HarvestLootRule LootRuleHarvest { get; set; }
    [Array(CountFrom = nameof(MemberInfosCount))] public List<GroupMember> MemberInfos { get; set; } = [];

    public Identity LeaderIdentity { get; set; } = new();
    [UInt(14)] public ushort RealmId { get; set; }     //< Why again? Tf?

    public GroupMarkerInfo MarkerInfo { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(Flags, 32);
        writer.Write(MemberInfos.Count);
        writer.Write(MaxGroupSize);

        writer.Write(LootRule, 3u);
        writer.Write(LootRuleThreshold, 3u);
        writer.Write(LootThreshold, 4u);
        writer.Write(LootRuleHarvest, 2u);

        MemberInfos.ForEach(member => member.Write(writer));

        LeaderIdentity.Write(writer);
        writer.Write(RealmId, 14);

        MarkerInfo.Write(writer);
    }
}
