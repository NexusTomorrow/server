using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01D0, Direction.ClientToServer)]
public sealed partial class ChatChannelSetPassword
{
    public ChatChannelKey Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
