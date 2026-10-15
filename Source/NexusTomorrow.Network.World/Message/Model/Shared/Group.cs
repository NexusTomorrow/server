using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Group
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
}
