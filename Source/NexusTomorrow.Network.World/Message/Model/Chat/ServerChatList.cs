using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChatList)]
[WireMessage(GameMessageOpcode.ServerChatList, Direction.ServerToClient)]
public sealed partial class ServerChatList : IWritable
{
    [UInt(14)] public ChatChannelType Type { get; set; }
    [UInt(64)] public ulong ChannelId { get; set; }
    [UInt(32), Count] public uint NamesCount { get; set; }
    [Array(CountFrom = nameof(NamesCount)), WideString] public List<string> Names { get; set; }
    [Array(CountFrom = nameof(NamesCount)), UInt(32)] public List<ChatChannelMemberFlags> Flags { get; set; }
    [NotOnWire] public bool More { get; set; } // not used by client

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 14u);
        writer.Write(ChannelId);

        writer.Write(Names.Count);
        foreach (string name in Names)
            writer.WriteStringWide(name);
        foreach (ChatChannelMemberFlags flag in Flags)
            writer.Write(flag, 32u);
    }
}
