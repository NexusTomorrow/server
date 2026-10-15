using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ClientCharacterDelete, Direction.ClientToServer)]
public sealed partial class ClientCharacterDelete
{
    [UInt(64)] public ulong CharacterId { get; private set; }
}
