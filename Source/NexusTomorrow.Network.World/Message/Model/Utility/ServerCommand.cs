using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Client03E0, Direction.ClientToServer)]
public sealed partial class ServerCommand
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
