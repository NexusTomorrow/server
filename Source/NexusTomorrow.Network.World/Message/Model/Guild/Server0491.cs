using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0491, Direction.ServerToClient)]
public sealed partial class Server0491
{
    [WideString] public string Unknown0 { get; set; } = "";
    [WideString] public string Unknown1 { get; set; } = "";
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
    [UInt(64)] public ulong Unknown4 { get; set; }
}
