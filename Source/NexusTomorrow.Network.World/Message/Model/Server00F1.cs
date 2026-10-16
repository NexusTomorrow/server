using NexusTomorrow.Game.Static.Setting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server00F1, Direction.ServerToClient)]
public sealed partial class Server00F1
{
    [UInt(2)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
