using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.Server0982, Direction.ServerToClient)]
public sealed partial class Server0982
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
