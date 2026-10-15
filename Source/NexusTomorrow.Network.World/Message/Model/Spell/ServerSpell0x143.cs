using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpell0x143)]
[WireMessage(GameMessageOpcode.ServerSpell0x143, Direction.ServerToClient)]
public sealed partial class ServerSpell0x143 : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
