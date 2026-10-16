using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server00D3, Direction.ServerToClient)]
public sealed partial class Server00D3
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
