using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellRemove
{
    [WireStruct]
    public sealed partial class TargetSpellDodgeResult
    {
        [UInt(32)] public uint TargetUnitId { get; set; }
        [UInt(4)] public byte SpellDodged { get; set; } = 0;
        [UInt(32)] public uint AdditionalSoundDelay { get; set; } = 0;
    }
    [UInt(32)] public uint CastingId { get; set; }
    [Array(CountBits = 32)] public List<TargetSpellDodgeResult> TargetDodgeResults { get; set; } = [];
}
