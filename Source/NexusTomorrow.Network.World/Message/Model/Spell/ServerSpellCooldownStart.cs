using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellCooldownStart)]
[WireMessage(GameMessageOpcode.ServerSpellCooldownStart, Direction.ServerToClient)]
public sealed partial class ServerSpellCooldownStart : IWritable
{
    public Cooldown Cooldown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Cooldown.Write(writer);
    }
}
