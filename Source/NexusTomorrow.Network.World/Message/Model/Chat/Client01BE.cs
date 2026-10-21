using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01BE, Direction.ClientToServer)]
public sealed partial class Client01BE
{
    public ChatChannelKey Unknown0 { get; private set; } = new();
}
