using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientCastDefaultAttack, Direction.ClientToServer)]
public sealed partial class ClientCastDefaultAttack
{
    [UInt(1)] public bool ButtonPressed { get; private set; }
}
