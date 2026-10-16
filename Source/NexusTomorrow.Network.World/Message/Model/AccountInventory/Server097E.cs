using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server097E, Direction.ServerToClient)]
public sealed partial class Server097E
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
