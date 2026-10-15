using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Changes the number of stacks of buffs applied to target
[Message(GameMessageOpcode.ServerSpellBuffsUpdate)]
[WireMessage(GameMessageOpcode.ServerSpellBuffsUpdate, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffsUpdate : IWritable
{
    [WireStruct]
    public sealed partial class BuffInfo : IWritable
    {
        [UInt(32)] public uint ServerUniqueId { get; set; }
        [UInt(32)] public uint TargetUnitId { get; set; }
        [UInt(32)] public uint BuffCount { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ServerUniqueId);
            writer.Write(TargetUnitId);
            writer.Write(BuffCount);
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
