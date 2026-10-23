using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0529, Direction.ClientToServer)]
public sealed partial class HousingRenameProperty
{
    public RealmScopedId Unknown0 { get; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
