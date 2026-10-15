using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellThresholdUpdate)]
public class ServerSpellThresholdUpdate : IWritable
{
    public uint Spell4Id { get; set; }
    public byte CurrentThreshold { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4Id, 18u);
        writer.Write(CurrentThreshold);
    }
}
