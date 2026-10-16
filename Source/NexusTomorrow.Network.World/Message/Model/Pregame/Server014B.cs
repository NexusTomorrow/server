using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server014B, Direction.ServerToClient)]
public sealed partial class Server014B
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; }
}
