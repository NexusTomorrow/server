using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Can be sent to client while player is in Game or on CharacterSelect screen.
[WireMessage(GameMessageOpcode.ServerPtrCharacterCopyQueued, Direction.ServerToClient)]
public sealed partial class ServerPtrCharacterCopyQueued
{
}
