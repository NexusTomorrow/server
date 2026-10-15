using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingPlacedResidencesList)]
[WireMessage(GameMessageOpcode.ServerHousingPlacedResidencesList, Direction.ServerToClient)]
public sealed partial class ServerHousingPlacedResidencesList : IWritable
{
    [WireStruct]
    public sealed partial class Residence : IWritable
    {
        public Identity ResidenceIdentity { get; set; }
        [WideString] public string PlayerName { get; set; }
        [UInt(32)] public uint PropertyIndex { get; set; }

        public void Write(GamePacketWriter writer)
        {
            ResidenceIdentity.Write(writer);
            writer.WriteStringWide(PlayerName);
            writer.Write(PropertyIndex);
        }
    }

    [Array(CountBits = 32)] public List<Residence> Residences { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Residences.Count);
        Residences.ForEach(r => r.Write(writer));
    }
}
