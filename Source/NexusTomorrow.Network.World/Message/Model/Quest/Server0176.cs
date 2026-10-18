using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Server0176, Direction.ServerToClient)]
public sealed partial class Server0176
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(3)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}
