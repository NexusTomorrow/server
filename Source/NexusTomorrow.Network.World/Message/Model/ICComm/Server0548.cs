using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Server0548, Direction.ServerToClient)]
public sealed partial class Server0548
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
}
