using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatFlag, Direction.ServerToClient)]
public sealed partial class ServerChatFlag
{
    public Channel ChatChannelId { get; set; }
    [UInt(64)] public ulong ChannelGuid { get; set; }
    [UInt(32)] public ChatChannelMemberFlags Flags { get; set; }
}
