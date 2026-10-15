using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerPetClearFlair, Direction.ServerToClient)]
public sealed partial class ServerPetClearFlair
{
}
