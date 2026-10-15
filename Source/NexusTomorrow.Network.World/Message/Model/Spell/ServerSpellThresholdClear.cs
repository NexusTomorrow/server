using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellThresholdClear)]
[WireMessage(GameMessageOpcode.ServerSpellThresholdClear, Direction.ServerToClient)]
public sealed partial class ServerSpellThresholdClear : IWritable
{
    [UInt(18)] public uint Spell4Id { get; set; }
    [UInt(1)] public bool ProcessCooldown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4Id, 18u);
        writer.Write(ProcessCooldown);
    }
}
