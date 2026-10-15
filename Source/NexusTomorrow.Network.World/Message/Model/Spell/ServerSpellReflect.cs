using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellReflect)]
[WireMessage(GameMessageOpcode.ServerSpellReflect, Direction.ServerToClient)]
public sealed partial class ServerSpellReflect : IWritable
{
    [UInt(32)] public uint SpellUniqueId { get; set; }
    [UInt(19)] public uint Spell4EffectId { get; set; } = 0;
    [UInt(32)] public uint CasterUnitId { get; set; } = 0;
    [UInt(32)] public uint TargetUnitId { get; set; } = 0;

    public SpellEffectResult SpellEffectResult { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellUniqueId);
        writer.Write(Spell4EffectId, 19);
        writer.Write(CasterUnitId);
        writer.Write(TargetUnitId);
        SpellEffectResult.Write(writer);
    }
}
