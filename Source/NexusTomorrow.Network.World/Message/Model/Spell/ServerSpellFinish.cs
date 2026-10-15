using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellFinish)]
[WireMessage(GameMessageOpcode.ServerSpellFinish, Direction.ServerToClient)]

public sealed partial class ServerSpellFinish : IWritable
{
    [UInt(32)] public uint ServerUniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ServerUniqueId);
    }
}
