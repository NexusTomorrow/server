using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server086F, Direction.ServerToClient)]
public sealed partial class Server086F
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(3)] public byte Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
