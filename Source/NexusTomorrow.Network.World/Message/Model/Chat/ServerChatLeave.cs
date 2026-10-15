using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatLeave, Direction.ServerToClient)]
public sealed partial class ServerChatLeave
{
    public Channel Channel { get; set; }
    [UInt(2)] public ChatChannelLeaveReason Leave { get; set; }
}
