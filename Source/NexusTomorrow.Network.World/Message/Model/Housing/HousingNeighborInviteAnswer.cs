using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0513, Direction.ClientToServer)]
public sealed partial class HousingNeighborInviteAnswer
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
