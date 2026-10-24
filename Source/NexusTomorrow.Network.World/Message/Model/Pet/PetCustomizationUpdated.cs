using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Adds or replaces one pet customization; the client raises PetCustomizationUpdated with it.
/// </summary>
[WireMessage(GameMessageOpcode.Server012F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetCustomizationUpdated
{
    /// <summary>
    /// The customization. The client replaces the one it holds under the same <see cref="PetCustomization.PetType"/> and
    /// <see cref="PetCustomization.ObjectId"/>, or adds it.
    /// </summary>
    public PetCustomization Customization { get; set; } = new();
}
