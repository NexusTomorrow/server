using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0414, Direction.ServerToClient)]
public sealed partial class GroupInstanceDifficultyChanged
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(2)] public byte Unknown2 { get; set; }
}
