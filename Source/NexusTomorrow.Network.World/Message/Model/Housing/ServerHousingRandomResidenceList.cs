using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingRandomResidenceList)]
public class ServerHousingRandomResidenceList : IWritable
{
    public class Residence : IWritable
    {
        public Identity ResidenceIdentity { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }

        public void Write(GamePacketWriter writer)
        {
            ResidenceIdentity.Write(writer);
            writer.WriteStringWide(Owner);
            writer.WriteStringWide(Name);
        }
    }

    public List<Residence> Residences { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Residences.Count);
        Residences.ForEach(c => c.Write(writer));
    }
}
