using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerPetStanceChanged, Direction.ServerToClient)]
public sealed partial class ServerPetStanceChanged
{
    [UInt(32)] public uint PetUnitId { get; set; } // 0 means all engineer pets
    [UInt(5)] public PetStance Stance { get; set; }
}
