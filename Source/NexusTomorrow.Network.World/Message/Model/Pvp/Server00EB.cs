using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.Server00EB, Direction.ServerToClient)]
public sealed partial class Server00EB
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
