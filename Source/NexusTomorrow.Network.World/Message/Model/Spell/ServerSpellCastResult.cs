using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellCastResult)]
[WireMessage(GameMessageOpcode.ServerSpellCastResult, Direction.ServerToClient)]
public sealed partial class ServerSpellCastResult : IWritable
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; set; }
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(9)] public CastResult CastResult { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ClientSpellCastUniqueId);
        writer.Write(Spell4Id, 18u);
        writer.Write(CastResult, 9u);
    }
}
