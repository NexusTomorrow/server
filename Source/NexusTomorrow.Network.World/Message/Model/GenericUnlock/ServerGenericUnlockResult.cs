using NexusTomorrow.Game.Static.GenericUnlock;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.Server0985, Direction.ServerToClient)]
public sealed partial class Server0985
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
