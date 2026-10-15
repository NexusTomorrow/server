using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ClientCharacterDelete)]
[WireMessage(GameMessageOpcode.ClientCharacterDelete, Direction.ClientToServer)]
public sealed partial class ClientCharacterDelete : IReadable
{
    [UInt(64)] public ulong CharacterId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CharacterId = reader.ReadULong();
    }
}
