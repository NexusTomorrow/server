using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Called from RequestCamp, ExitGame, Camp, CancelExit lua functions
[WireMessage(GameMessageOpcode.ClientLogoutRequest, Direction.ClientToServer)]
public sealed partial class ClientLogoutRequest
{
    [UInt(1)] public bool Initiated { get; private set; }
}
