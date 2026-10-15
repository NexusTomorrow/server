using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientActivateUnit)]
public class ClientActivateUnit : IReadable
{
    public uint UnitId { get; private set; } // Interaction unit

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
    }
}
