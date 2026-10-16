using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server00B2, Direction.ServerToClient)]
public sealed partial class Server00B2
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
