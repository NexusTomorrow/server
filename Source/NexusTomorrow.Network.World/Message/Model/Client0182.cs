using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0182, Direction.ClientToServer)]
public sealed partial class Client0182
{
    public Server0111Part2 Unknown0 { get; } = new();
    public Server0111Part2 Unknown1 { get; } = new();
}
