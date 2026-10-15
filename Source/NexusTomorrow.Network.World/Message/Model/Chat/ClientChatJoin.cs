using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatJoin)]
[WireMessage(GameMessageOpcode.ClientChatJoin, Direction.ClientToServer)]
public sealed partial class ClientChatJoin : IReadable
{
    [UInt(14)] public ChatChannelType Type { get; private set; }
    [WideString] public string Name { get; private set; }
    [WideString] public string Password { get; private set; }
    [UInt(32)] public uint Order { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<ChatChannelType>(14u);
        Name = reader.ReadWideString();
        Password = reader.ReadWideString();
        Order = reader.ReadUInt();
    }
}
