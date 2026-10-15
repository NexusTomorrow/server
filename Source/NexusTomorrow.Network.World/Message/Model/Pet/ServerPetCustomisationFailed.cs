using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerPetCustomizationFailed, Direction.ServerToClient)]
public sealed partial class ServerPetCustomisationFailed
{
    [UInt(4)] public PetCustomizeResult Reason { get; set; }
    [UInt(2)] public PetType Type { get; set; }
    [UInt(32)] public uint PetUnitId { get; set; }
    [UInt(16)] public ushort FlairSlotIndex { get; set; }
    [UInt(14)] public ushort PetFlairId { get; set; }
}
