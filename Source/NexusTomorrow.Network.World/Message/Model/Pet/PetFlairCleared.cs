using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server023B, Direction.ServerToClient)]
public sealed partial class PetFlairCleared
{
}
