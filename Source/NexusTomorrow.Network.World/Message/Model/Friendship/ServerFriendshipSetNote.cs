using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03C9, Direction.ServerToClient)]
public sealed partial class Server03C9
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; }
}
