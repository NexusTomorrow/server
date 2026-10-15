using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ClientChatJoin, Direction.ClientToServer)]
public sealed partial class ClientChatJoin
{
    [UInt(14)] public ChatChannelType Type { get; private set; }
    [WideString] public string Name { get; private set; }
    [WideString] public string Password { get; private set; }
    [UInt(32)] public uint Order { get; private set; }
}
