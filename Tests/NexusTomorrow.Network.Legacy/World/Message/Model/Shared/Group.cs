using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

public class Group : IWritable
{
    public ulong GroupId { get; set; }
    public GroupFlags Flags { get; set; }
    public List<GroupMember> MemberInfos { get; set; } = [];
    public uint MaxGroupSize { get; set; }

    public LootRule LootRule { get; set; }
    public LootRule LootRuleThreshold { get; set; }
    public LootThreshold LootThreshold { get; set; }
    public HarvestLootRule LootRuleHarvest { get; set; }

    public Identity LeaderIdentity { get; set; } = new();
    public ushort RealmId { get; set; }     //< Why again? Tf?

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
