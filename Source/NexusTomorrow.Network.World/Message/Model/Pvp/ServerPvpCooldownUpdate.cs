using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Time for PVP flag to expire in milliseconds
[WireMessage(GameMessageOpcode.ServerPvpCooldownUpdate, Direction.ServerToClient)]
public sealed partial class ServerPvpCooldownUpdate
{
    [UInt(32)] public uint CooldownRemaining { get; set; }
}
