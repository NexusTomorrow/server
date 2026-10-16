using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03A6, Direction.ServerToClient)]
public sealed partial class Server03A6
{
    [WideString] public string Unknown0 { get; set; } = "";
    [WideString] public string Unknown1 { get; set; } = "";
    [UInt(3)] public byte Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}
