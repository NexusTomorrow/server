using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Server0362, Direction.ServerToClient)]
public sealed partial class Server0362
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
}
