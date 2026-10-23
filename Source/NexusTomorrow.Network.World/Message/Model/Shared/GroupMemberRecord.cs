using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupMemberRecord
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    public GroupMemberInfo Unknown2 { get; set; } = new();
    [UInt(32)] public uint Unknown3 { get; set; }
}
