using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01CE, Direction.ClientToServer)]
public sealed partial class ChatChannelSetMute
{
    public ChatChannelKey Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
    [UInt(1)] public bool Unknown2 { get; private set; }
}
