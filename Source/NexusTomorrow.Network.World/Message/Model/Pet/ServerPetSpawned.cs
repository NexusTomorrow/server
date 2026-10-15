using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[Message(GameMessageOpcode.ServerPetSpawned)]
[WireMessage(GameMessageOpcode.ServerPetSpawned, Direction.ServerToClient)]
public sealed partial class ServerPetSpawned : IWritable
{
    [UInt(32)] public uint PetUnitId { get; set; } // 0 means all engineer pets
    [UInt(18)] public uint SummoningSpell4Id { get; set; }
    [UInt(5)] public uint ValidStances { get; set; }
    [UInt(5)] public uint Stance { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PetUnitId);
        writer.Write(SummoningSpell4Id, 18u);
        writer.Write(ValidStances, 5u);
        writer.Write(Stance, 5u);
    }
}
