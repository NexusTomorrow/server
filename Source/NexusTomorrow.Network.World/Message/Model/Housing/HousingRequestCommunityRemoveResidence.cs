using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client04EA, Direction.ClientToServer)]
public sealed partial class HousingRequestCommunityRemoveResidence
{
    public RealmScopedId Unknown0 { get; private set; } = new();
}
