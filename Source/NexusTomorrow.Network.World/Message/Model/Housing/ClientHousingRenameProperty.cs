using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingRenameProperty, Direction.ClientToServer)]
public sealed partial class ClientHousingRenameProperty
{
    public Identity TargetResidence { get; } = new();
    [WideString] public string Name { get; private set; }
}
