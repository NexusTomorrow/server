using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server024A, Direction.ServerToClient)]
public sealed partial class Server024A
{
    public Server024APart0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
