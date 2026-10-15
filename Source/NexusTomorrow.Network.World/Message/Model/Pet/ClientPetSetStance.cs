using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[Message(GameMessageOpcode.ClientPetSetStance)]
[WireMessage(GameMessageOpcode.ClientPetSetStance, Direction.ClientToServer)]
public sealed partial class ClientPetSetStance : IReadable
{
    [UInt(32)] public uint PetUnitId { get; private set; } //The id number for the pet whose stance is being set. Setting this to 0 will set the stance for all of the player's pets.
    [UInt(8)] public PetStance Stance { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PetUnitId = reader.ReadUInt();
        Stance = reader.ReadEnum<PetStance>(8u);
    }
}
