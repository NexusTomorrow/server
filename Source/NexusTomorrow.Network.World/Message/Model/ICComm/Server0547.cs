using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Server0547, Direction.ServerToClient)]
public sealed partial class Server0547
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; }
}
