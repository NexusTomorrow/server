using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.Server0179, Direction.ServerToClient)]
public sealed partial class Server0179
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
