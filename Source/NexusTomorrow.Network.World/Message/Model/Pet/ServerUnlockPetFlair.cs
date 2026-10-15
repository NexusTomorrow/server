using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[Message(GameMessageOpcode.ServerUnlockPetFlair)]
[WireMessage(GameMessageOpcode.ServerUnlockPetFlair, Direction.ServerToClient)]
public sealed partial class ServerUnlockPetFlair : IWritable
{
    [UInt(14)] public ushort PetFlairId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PetFlairId, 14u);
    }
}
