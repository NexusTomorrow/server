using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server00FA, Direction.ServerToClient)]
public sealed partial class Server00FA
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
