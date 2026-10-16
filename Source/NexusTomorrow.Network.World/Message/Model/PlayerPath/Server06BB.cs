using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06BB, Direction.ServerToClient)]
public sealed partial class Server06BB
{
    public Server06B5Part0 Unknown0 { get; set; } = new();
}
