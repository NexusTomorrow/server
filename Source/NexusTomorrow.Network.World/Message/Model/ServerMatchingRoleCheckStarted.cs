using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingRoleCheckStarted, Direction.ServerToClient)]
public sealed partial class ServerMatchingRoleCheckStarted
{
    [UInt(1)] public bool RolesRequired { get; set; }
}
