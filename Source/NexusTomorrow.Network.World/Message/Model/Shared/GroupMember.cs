using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupMember
{
    public Identity MemberIdentity { get; set; } = new();
    [UInt(32)] public GroupMemberInfoFlags Flags { get; set; }
    public GroupCharacter Member { get; set; }
    [UInt(32)] public uint GroupIndex { get; set; }
}
