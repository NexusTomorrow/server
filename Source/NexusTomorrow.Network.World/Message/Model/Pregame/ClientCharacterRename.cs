using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ClientCharacterRename)]
[WireMessage(GameMessageOpcode.ClientCharacterRename, Direction.ClientToServer)]
public sealed partial class ClientCharacterRename : IReadable
{
    [UInt(64)] public ulong CharacterId { get; private set; }
    [WideString] public string Name { get; private set; }

    public void Read(GamePacketReader reader)
    {
        CharacterId = reader.ReadULong();
        Name = reader.ReadWideString();
    }
}
