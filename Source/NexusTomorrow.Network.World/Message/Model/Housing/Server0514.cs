using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0514, Direction.ServerToClient)]
public sealed partial class Server0514
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [WideString] public string Unknown1 { get; set; } = "";
}
