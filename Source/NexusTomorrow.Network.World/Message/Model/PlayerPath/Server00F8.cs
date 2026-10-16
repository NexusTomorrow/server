using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server00F8, Direction.ServerToClient)]
public sealed partial class Server00F8
{
    [UInt(14)] public ushort Unknown0 { get; set; }
}
