using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellThresholdUpdate)]
[WireMessage(GameMessageOpcode.ServerSpellThresholdUpdate, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdUpdate : IWritable
{
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(8)] public byte CurrentThreshold { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4Id, 18u);
        writer.Write(CurrentThreshold);
    }
}
