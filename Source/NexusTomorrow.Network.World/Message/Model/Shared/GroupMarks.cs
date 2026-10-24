using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupMarks
{
    [Array(CountBits = 32)] public GroupMark[] Unknown0 { get; set; } = [];
}
