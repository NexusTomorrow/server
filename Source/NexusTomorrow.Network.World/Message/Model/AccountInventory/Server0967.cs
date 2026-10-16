using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0967, Direction.ServerToClient)]
public sealed partial class Server0967
{
    public Server0966Part0 Unknown0 { get; set; } = new();
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
}
