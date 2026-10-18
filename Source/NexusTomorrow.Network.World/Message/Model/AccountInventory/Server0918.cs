using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0918, Direction.ServerToClient)]
public sealed partial class Server0918
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
