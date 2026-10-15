using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastDefaultAttack)]
[WireMessage(GameMessageOpcode.ClientCastDefaultAttack, Direction.ClientToServer)]
public sealed partial class ClientCastDefaultAttack : IReadable
{
    [UInt(1)] public bool ButtonPressed { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ButtonPressed = reader.ReadBit();
    }
}
