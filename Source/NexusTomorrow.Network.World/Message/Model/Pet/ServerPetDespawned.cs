using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[Message(GameMessageOpcode.ServerPetDespawned)]
[WireMessage(GameMessageOpcode.ServerPetDespawned, Direction.ServerToClient)]
public sealed partial class ServerPetDespawned : IWritable
{
    [UInt(32)] public uint PetUnitId { get; set; } // TBC if this follows other messages, 0 means all engineer pets

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PetUnitId);
    }
}
