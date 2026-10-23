using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Chat.Model;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01C2, Direction.ServerToClient)]
public sealed partial class ChatSendAccepted
{
    [UInt(16)] public ushort Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [Array(CountBits = 5)] public List<Client01C3Union0Arm08> Unknown2 { get; } = [];
    [WideString] public string Unknown3 { get; set; } = "";
    [WideString] public string Unknown4 { get; set; } = "";
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(8)] public byte Unknown6 { get; set; }
}
