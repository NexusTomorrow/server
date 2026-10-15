using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerCooldownList)]
[WireMessage(GameMessageOpcode.ServerCooldownList, Direction.ServerToClient)]
public sealed partial class ServerCooldownList : IWritable
{
    [Array(CountBits = 32)] public List<Cooldown> Cooldowns { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Cooldowns.Count);
        Cooldowns.ForEach(c => c.Write(writer));
    }
}
