using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientActivateUnit)]
[WireMessage(GameMessageOpcode.ClientActivateUnit, Direction.ClientToServer)]
public sealed partial class ClientActivateUnit : IReadable
{
    [UInt(32)] public uint UnitId { get; private set; } // Interaction unit

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
    }
}
