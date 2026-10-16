using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server06B3, Direction.ServerToClient)]
public sealed partial class Server06B3
{
    [UInt(8)] public byte Unknown0 { get; set; }
}
