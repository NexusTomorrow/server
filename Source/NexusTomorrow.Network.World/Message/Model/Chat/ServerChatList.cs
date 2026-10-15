using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChatList, Direction.ServerToClient)]
public sealed partial class ServerChatList
{
    [UInt(14)] public ChatChannelType Type { get; set; }
    [UInt(64)] public ulong ChannelId { get; set; }
    [UInt(32), Count] public uint NamesCount { get; set; }
    [Array(CountFrom = nameof(NamesCount)), WideString] public List<string> Names { get; set; }
    [Array(CountFrom = nameof(NamesCount)), UInt(32)] public List<ChatChannelMemberFlags> Flags { get; set; }
    [NotOnWire] public bool More { get; set; } // not used by client
}
