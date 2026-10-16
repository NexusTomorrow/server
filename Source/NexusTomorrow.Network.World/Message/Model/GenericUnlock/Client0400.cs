using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.Client0400, Direction.ClientToServer)]
public sealed partial class Client0400
{
    public Server0111Part2 Unknown0 { get; } = new();
}
