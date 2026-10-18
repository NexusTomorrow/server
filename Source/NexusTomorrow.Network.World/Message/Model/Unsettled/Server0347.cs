using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0347, Direction.ServerToClient)]
public sealed partial class Server0347
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
}
