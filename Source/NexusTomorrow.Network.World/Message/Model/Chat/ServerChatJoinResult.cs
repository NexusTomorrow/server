using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatJoinResult, Direction.ServerToClient)]
public sealed partial class ServerChatJoinResult
{
    [UInt(14)] public ChatChannelType ChatChannelId { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(5)] public ChatResult Result { get; set; }
}
