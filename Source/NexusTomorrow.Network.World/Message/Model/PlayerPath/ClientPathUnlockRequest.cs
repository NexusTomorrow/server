using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathUnlockRequest, Direction.ClientToServer)]
public sealed partial class ClientPathUnlockRequest
{
    [UInt(3)] public Path Path { get; private set; }
}
