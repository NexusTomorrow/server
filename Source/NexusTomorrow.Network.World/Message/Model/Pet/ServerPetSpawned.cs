using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.ServerPetSpawned, Direction.ServerToClient)]
public sealed partial class ServerPetSpawned
{
    [UInt(32)] public uint PetUnitId { get; set; } // 0 means all engineer pets
    [UInt(18)] public uint SummoningSpell4Id { get; set; }
    [UInt(5)] public uint ValidStances { get; set; }
    [UInt(5)] public uint Stance { get; set; }
}
