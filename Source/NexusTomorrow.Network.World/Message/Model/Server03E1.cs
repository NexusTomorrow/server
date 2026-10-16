using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server03E1, Direction.ServerToClient)]
public sealed partial class Server03E1
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
