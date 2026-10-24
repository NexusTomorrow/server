using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server012F, Direction.ServerToClient)]
public sealed partial class PetCustomizationUpdated
{
    public PetCustomization Unknown0 { get; set; } = new();
}
