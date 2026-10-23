using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01C3, Direction.ClientToServer)]
public sealed partial class ChatSend
{
    public ChatChannelKey Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
    [Array(CountBits = 5)] public List<Client01C3Part0> Unknown2 { get; } = [];
    [UInt(16)] public ushort Unknown3 { get; private set; }
}
