using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpell0x143, Direction.ServerToClient)]
public sealed partial class ServerSpell0x143
{
}
