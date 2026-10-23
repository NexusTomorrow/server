using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0519, Direction.ServerToClient)]
public sealed partial class HousingNeighborUpdate
{
    public HousingNeighbor Unknown0 { get; set; } = new();
    [UInt(3)] public byte Unknown1 { get; set; }
}
