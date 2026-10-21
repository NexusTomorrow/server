using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01D2, Direction.ClientToServer)]
public sealed partial class Client01D2
{
    public ChatChannelKey Unknown0 { get; private set; } = new();
}
