using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellBuffRemoveSingle)]
[WireMessage(GameMessageOpcode.ServerSpellBuffRemoveSingle, Direction.ServerToClient)]
public sealed partial class ServerSpellBuffRemoveSingle : IWritable
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerUniqueId);
        writer.Write(TargetUnitId);
    }
}
