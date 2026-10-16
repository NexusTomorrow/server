using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server089B, Direction.ServerToClient)]
public sealed partial class Server089B
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(2)] public byte Unknown2 { get; set; }
    [UInt(3)] public byte Unknown3 { get; set; }
}
