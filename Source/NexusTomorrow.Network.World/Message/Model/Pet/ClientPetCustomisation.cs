using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ClientPetCustomization, Direction.ClientToServer)]
public sealed partial class ClientPetCustomisation
{
    [UInt(2)] public PetType PetType { get; private set; }
    [UInt(32)] public uint PetObjectId { get; private set; } // PathScientistScanBotProfileId in the case of scanbots
    [UInt(16)] public ushort FlairSlotIndex { get; private set; }
    [UInt(14)] public ushort FlairId { get; private set; }
}
