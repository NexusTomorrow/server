using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ClientCharacterRename, Direction.ClientToServer)]
public sealed partial class ClientCharacterRename
{
    [UInt(64)] public ulong CharacterId { get; private set; }
    [WideString] public string Name { get; private set; }
}
