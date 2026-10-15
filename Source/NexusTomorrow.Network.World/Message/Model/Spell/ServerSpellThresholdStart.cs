using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellThresholdStart)]
[WireMessage(GameMessageOpcode.ServerSpellThresholdStart, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdStart : IWritable
{
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(18)] public uint RootSpell4Id { get; set; }
    [UInt(18)] public uint ParentSpell4Id { get; set; } = 0;
    [UInt(32)] public uint ServerUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4Id, 18u);
        writer.Write(RootSpell4Id, 18u);
        writer.Write(ParentSpell4Id, 18u);
        writer.Write(ServerUniqueId);
    }
}
