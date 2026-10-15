using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[Message(GameMessageOpcode.ServerPetCustomizationList)]
public class ServerPetCustomisationList : IWritable
{
    public NetworkBitArray UnlockedFlair { get; set; } = new(512, NetworkBitArray.BitOrder.MostSignificantBit);
    public List<PetCustomisation> PetCustomisations { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.WriteBytes(UnlockedFlair.GetBuffer());

        writer.Write(PetCustomisations.Count, 32u);
        foreach (PetCustomisation petCustomisation in PetCustomisations)
            petCustomisation.Write(writer);
    }
}
