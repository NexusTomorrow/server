using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0467, Direction.ServerToClient)]
public sealed partial class Server0467
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    public Client00B5Part0 Unknown1 { get; set; }
    [UInt(14)] public uint Unknown2 { get; set; }
    [UInt(15)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
}
