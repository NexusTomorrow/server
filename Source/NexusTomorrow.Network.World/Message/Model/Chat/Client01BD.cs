using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Client01BD, Direction.ClientToServer)]
public sealed partial class Client01BD
{
    public Server01B5Part0 Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
