using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerPetCustomization, Direction.ServerToClient)]
public sealed partial class ServerPetCustomisation
{
    public PetCustomisation PetCustomisation { get; set; }
}
