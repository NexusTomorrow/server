using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Server0257, Direction.ServerToClient)]
public sealed partial class Server0257
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(4)] public byte Unknown2 { get; set; }
}
