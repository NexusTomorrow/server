using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Message003D, Direction.Both)]
[Dormant]
public sealed partial class Message003D
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
    [UInt(64)] public ulong Unknown3 { get; set; }
}
