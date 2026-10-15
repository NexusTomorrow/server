using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatJoin, Direction.ServerToClient)]
public sealed partial class ServerChatJoin
{
    public Channel Channel { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(32)] public uint MemberCount { get; set; }
    [UInt(32)] public ChatChannelMemberFlags Flags { get; set; }
    [UInt(32)] public uint Order { get; set; }
}
