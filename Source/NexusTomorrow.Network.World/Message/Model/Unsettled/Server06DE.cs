using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server06DE, Direction.ServerToClient)]
public sealed partial class Server06DE
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
