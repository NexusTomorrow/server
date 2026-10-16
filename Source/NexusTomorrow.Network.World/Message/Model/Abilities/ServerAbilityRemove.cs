using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server014A, Direction.ServerToClient)]
public sealed partial class Server014A
{
    public Server0111Part2 Unknown0 { get; set; } = new();
}
