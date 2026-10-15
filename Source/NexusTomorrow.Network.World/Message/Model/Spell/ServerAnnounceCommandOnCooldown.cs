using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerAnnounceCommandOnCooldown)]
[WireMessage(GameMessageOpcode.ServerAnnounceCommandOnCooldown, Direction.ServerToClient)]
public sealed partial class ServerAnnounceCommandOnCooldown : IWritable
{
    [UInt(32)] public uint CooldownTime { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CooldownTime);
    }
}
