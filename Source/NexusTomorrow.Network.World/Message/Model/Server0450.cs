using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0450, Direction.ServerToClient)]
public sealed partial class Server0450
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    public Client00B5Part0 Unknown2 { get; set; } = new();
    [UInt(4)] public byte Unknown3 { get; set; }
}
