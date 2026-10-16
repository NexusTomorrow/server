using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server059D, Direction.ServerToClient)]
public sealed partial class Server059D
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
