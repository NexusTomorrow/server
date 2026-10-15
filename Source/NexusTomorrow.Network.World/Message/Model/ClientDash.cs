using NexusTomorrow.Game.Static.Entity.Movement;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientDash, Direction.ClientToServer)]
public sealed partial class ClientDash
{
    [UInt(3)] public DashDirection DashDirection { get; set; }
}
