using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0149, Direction.ClientToServer)]
public sealed partial class Client0149
{
    public Server0111Part2 Unknown0 { get; } = new();
}
