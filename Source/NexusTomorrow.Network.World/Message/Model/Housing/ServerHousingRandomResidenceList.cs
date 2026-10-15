using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingRandomResidenceList)]
[WireMessage(GameMessageOpcode.ServerHousingRandomResidenceList, Direction.ServerToClient)]
public sealed partial class ServerHousingRandomResidenceList : IWritable
{
    [WireStruct]
    public sealed partial class Residence : IWritable
    {
        public Identity ResidenceIdentity { get; set; }
        [WideString] public string Owner { get; set; }
        [WideString] public string Name { get; set; }

        public void Write(GamePacketWriter writer)
        {
            ResidenceIdentity.Write(writer);
            writer.WriteStringWide(Owner);
            writer.WriteStringWide(Name);
        }
    }

    [Array(CountBits = 32)] public List<Residence> Residences { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Residences.Count);
        Residences.ForEach(c => c.Write(writer));
    }
}
