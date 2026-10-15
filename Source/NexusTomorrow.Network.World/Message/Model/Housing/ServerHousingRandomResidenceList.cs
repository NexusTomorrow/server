using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingRandomResidenceList, Direction.ServerToClient)]
public sealed partial class ServerHousingRandomResidenceList
{
    [WireStruct]
    public sealed partial class Residence
    {
        public Identity ResidenceIdentity { get; set; }
        [WideString] public string Owner { get; set; }
        [WideString] public string Name { get; set; }
    }

    [Array(CountBits = 32)] public List<Residence> Residences { get; set; } = [];
}
