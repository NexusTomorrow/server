using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerAbilityBook)]
[WireMessage(GameMessageOpcode.ServerAbilityBook, Direction.ServerToClient)]
public sealed partial class ServerAbilities : IWritable
{
    [WireStruct]
    public sealed partial class Spell : IWritable
    {
        [UInt(18)] public uint Spell4BaseId { get; set; }
        [UInt(4)] public byte TierIndexAchieved { get; set; }
        [UInt(3)] public byte SpecIndex { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Spell4BaseId, 18u);
            writer.Write(TierIndexAchieved, 4u);
            writer.Write(SpecIndex, 3u);
        }
    }

    [Array(CountBits = 32)] public List<Spell> Spells { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write((uint)Spells.Count);
        Spells.ForEach(e => e.Write(writer));
    }
}
