using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01BB, Direction.ServerToClient)]
public sealed partial class ChatJoinChannelResult
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
    [UInt(5)] public byte Unknown2 { get; set; }
}
