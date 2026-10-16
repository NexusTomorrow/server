using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server0861, Direction.ServerToClient)]
public sealed partial class Server0861
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
