using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerPetCustomizationList, Direction.ServerToClient)]
public sealed partial class ServerPetCustomisationList
{
    [FixedArray(64), UInt(8)] public byte[] UnlockedFlair { get; set; } = new byte[64]; // 512 flags, most significant bit of each byte first
    [Array(CountBits = 32)] public List<PetCustomisation> PetCustomisations { get; set; } = [];
}
