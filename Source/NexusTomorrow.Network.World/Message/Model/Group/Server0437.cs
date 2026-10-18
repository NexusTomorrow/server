using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0437, Direction.ServerToClient)]
public sealed partial class Server0437
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    public Client00B5Part0 Unknown2 { get; set; } = new();
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
}
