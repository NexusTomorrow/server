using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Changes the number of stacks of buffs applied to target
[WireMessage(GameMessageOpcode.ServerSpellBuffsUpdate, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffsUpdate
{
    [WireStruct]
    public sealed partial class BuffInfo
    {
        [UInt(32)] public uint ServerUniqueId { get; set; }
        [UInt(32)] public uint TargetUnitId { get; set; }
        [UInt(32)] public uint BuffCount { get; set; }
    };

    [Array(CountBits = 32)] public List<BuffInfo> Buffs { get; set; } = [];
}
