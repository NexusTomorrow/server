using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01D9, Direction.ServerToClient)]
public sealed partial class Server01D9
{
    [WideString] public string Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
}
