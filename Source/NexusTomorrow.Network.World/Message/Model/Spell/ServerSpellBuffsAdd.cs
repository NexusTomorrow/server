using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Triggers buffs and spelleffects from ActiveSpell on the client to be applied to the target.
[WireMessage(GameMessageOpcode.ServerSpellBuffsAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffsAdd
{
    [WireStruct]
    public sealed partial class BuffInfo
    {
        [UInt(32)] public uint ServerUniqueId { get; set; }
        [UInt(32)] public uint TargetUnitId { get; set; }
        [UInt(32)] public uint Unused { get; set; }
    };

    [Array(CountBits = 32)] public List<BuffInfo> Buffs { get; set; } = [];
}
