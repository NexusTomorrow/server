using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.Server054E, Direction.ServerToClient)]
public sealed partial class Server054E
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [NarrowString] public string Unknown1 { get; set; } = "";
    [WideString] public string Unknown2 { get; set; } = "";
}

