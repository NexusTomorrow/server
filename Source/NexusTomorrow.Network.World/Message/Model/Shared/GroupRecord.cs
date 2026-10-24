using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupRecord
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32), Count] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }

    [UInt(3)] public byte Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
    [UInt(4)] public byte Unknown6 { get; set; }
    [UInt(2)] public byte Unknown7 { get; set; }
    [Array(CountFrom = nameof(Unknown2))] public List<GroupMemberRecord> Unknown8 { get; set; } = [];

    public RealmScopedId Unknown9 { get; set; } = new();
    [UInt(14)] public ushort Unknown10 { get; set; }

    public GroupMarks Unknown11 { get; set; } = new();
}
