using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingPlacedResidencesList, Direction.ServerToClient)]
public sealed partial class ServerHousingPlacedResidencesList
{
    [WireStruct]
    public sealed partial class Residence
    {
        public Identity ResidenceIdentity { get; set; }
        [WideString] public string PlayerName { get; set; }
        [UInt(32)] public uint PropertyIndex { get; set; }
    }

    [Array(CountBits = 32)] public List<Residence> Residences { get; set; } = [];
}
