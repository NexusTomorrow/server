using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerUnlockPetFlair, Direction.ServerToClient)]
public sealed partial class ServerUnlockPetFlair
{
    [UInt(14)] public ushort PetFlairId { get; set; }
}
