using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Combat;

public class CombatLogCastData : IWritable
{
    public uint CasterId { get; set; }
    public uint TargetId { get; set; }
    public uint SpellId { get; set; } // 18u
    public CombatResult CombatResult { get; set; } // 4u

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterId);
        writer.Write(TargetId);
        writer.Write(SpellId, 18u);
        writer.Write(CombatResult, 4u);
    }
}
