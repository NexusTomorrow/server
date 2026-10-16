using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0100, Direction.ServerToClient)]
public sealed partial class Server0100
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
}
