using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Server0967, Direction.ServerToClient)]
public sealed partial class Server0967
{
    public AccountCurrency Unknown0 { get; set; } = new();
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
}
