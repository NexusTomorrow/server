using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096A, Direction.ServerToClient)]
public sealed partial class Server096A
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Server096APart0 Unknown1 { get; set; }
}
