using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellRemove)]
[WireMessage(GameMessageOpcode.ServerSpellRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellRemove : IWritable
{
    [WireStruct]
    public sealed partial class TargetSpellDodgeResult : IWritable
    {
        [UInt(32)] public uint TargetUnitId { get; set; }
        [UInt(4)] public byte SpellDodged { get; set; } = 0;
        [UInt(32)] public uint AdditionalSoundDelay { get; set; } = 0;

        public void Write(GamePacketWriter writer)
        {
            writer.Write(TargetUnitId);
            writer.Write(SpellDodged, 4u);
            writer.Write(AdditionalSoundDelay);
        }
    }
    [UInt(32)] public uint CastingId { get; set; }
    [Array(CountBits = 32)] public List<TargetSpellDodgeResult> TargetDodgeResults { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CastingId);
        writer.Write(TargetDodgeResults.Count, 32u);
        TargetDodgeResults.ForEach(u => u.Write(writer));
    }
}
