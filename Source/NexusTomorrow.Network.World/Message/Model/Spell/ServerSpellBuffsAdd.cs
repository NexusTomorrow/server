using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Triggers buffs and spelleffects from ActiveSpell on the client to be applied to the target.
[Message(GameMessageOpcode.ServerSpellBuffsAdd)]
[WireMessage(GameMessageOpcode.ServerSpellBuffsAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffsAdd : IWritable
{
    [WireStruct]
    public sealed partial class BuffInfo : IWritable
    {
        [UInt(32)] public uint ServerUniqueId { get; set; }
        [UInt(32)] public uint TargetUnitId { get; set; }
        [UInt(32)] public uint Unused { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ServerUniqueId);
            writer.Write(TargetUnitId);
            writer.Write(Unused);
        }
    };

    [Array(CountBits = 32)] public List<BuffInfo> Buffs { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Buffs.Count);
        foreach (BuffInfo buff in Buffs)
        {
            buff.Write(writer);
        }
    }
}
