using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastDefaultAttack)]
public class ClientCastDefaultAttack : IReadable
{
    public bool ButtonPressed { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ButtonPressed = reader.ReadBit();
    }
}
