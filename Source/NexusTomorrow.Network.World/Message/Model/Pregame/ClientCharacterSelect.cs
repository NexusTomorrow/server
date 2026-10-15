using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent when the lua function CharacterScreenLib::SelectCharacter is called.
[Message(GameMessageOpcode.ClientCharacterSelect)]
[WireMessage(GameMessageOpcode.ClientCharacterSelect, Direction.ClientToServer)]
public sealed partial class ClientCharacterSelect : IReadable
{
    [UInt(64)] public ulong CharacterId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CharacterId = reader.ReadULong();
    }
}
