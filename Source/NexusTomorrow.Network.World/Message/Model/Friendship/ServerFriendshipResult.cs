using NexusTomorrow.Game.Static.Friendship;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.ServerFriendshipResult, Direction.ServerToClient)]
public sealed partial class ServerFriendshipResult
{
    [WideString] public string Message { get; set; } // In UI causes Event_FireGenericEvent("GenericEvent_SystemChannelMessage", strMessage)
    [UInt(6)] public FriendshipResult Result { get; set; }
}
