using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellAuraCancel)]
[WireMessage(GameMessageOpcode.ServerSpellAuraCancel, Direction.ServerToClient)]
public sealed partial class ServerSpellAuraCancel : IWritable
{
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(32)] public uint ServerUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TargetUnitId);
        writer.Write(ServerUniqueId);
    }
}
