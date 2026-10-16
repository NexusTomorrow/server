using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.Server075F, Direction.ServerToClient)]
public sealed partial class Server075F
{
    [UInt(15)] public ushort Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
}
