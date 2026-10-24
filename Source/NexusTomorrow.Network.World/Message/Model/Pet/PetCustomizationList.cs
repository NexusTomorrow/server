using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Server012E, Direction.ServerToClient)]
public sealed partial class PetCustomizationList
{
    [FixedArray(64), UInt(8)] public byte[] Unknown0 { get; set; } = new byte[64];
    [Array(CountBits = 32)] public List<PetCustomization> Unknown1 { get; set; } = [];
}
