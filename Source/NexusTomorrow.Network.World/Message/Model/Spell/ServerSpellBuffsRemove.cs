using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellBuffRemove)] // removes same buff base on the spell CastingId from a list of targets
[WireMessage(GameMessageOpcode.ServerSpellBuffRemove, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffRemove : IWritable
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Targets { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerUniqueId);
        writer.Write(Targets.Count, 32u);
        Targets.ForEach(c => writer.Write(c));
    }
}
