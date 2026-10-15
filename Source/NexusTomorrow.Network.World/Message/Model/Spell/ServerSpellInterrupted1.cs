using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellInterrupted1)]
[WireMessage(GameMessageOpcode.ServerSpellInterrupted1, Direction.ServerToClient)]
public sealed partial class ServerSpellInterrupted1 : IWritable
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(9)] public CastResult CastResult { get; set; }
    [UInt(32)] public uint CasterUnitId { get; set; }
    [UInt(1)] public bool CancelCast { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerUniqueId);
        writer.Write(CastResult, 9u);
        writer.Write(CasterUnitId);
        writer.Write(CancelCast);
    }
}
