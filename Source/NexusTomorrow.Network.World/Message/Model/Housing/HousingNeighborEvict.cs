using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0515, Direction.ClientToServer)]
public sealed partial class HousingNeighborEvict
{
    public RealmScopedId Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
