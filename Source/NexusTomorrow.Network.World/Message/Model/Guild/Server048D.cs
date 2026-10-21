using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server048D, Direction.ServerToClient)]
public sealed partial class Server048D
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
}
