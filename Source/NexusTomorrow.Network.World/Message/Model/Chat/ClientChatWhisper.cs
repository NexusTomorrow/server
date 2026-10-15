using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChatWhisper)]
[WireMessage(GameMessageOpcode.ClientChatWhisper, Direction.ClientToServer)]
public sealed partial class ClientChatWhisper : IReadable
{
    [WideString] public string ToName { get; private set; }
    [WideString] public string ToRealmName { get; private set; }

    [WideString] public string Message { get; private set; }
    [Array(CountBits = 5)] public List<ChatClientFormat> Formats { get; } = [];
    [UInt(16)] public ushort ChatMessageId { get; set; } // incremented by the client

    [UInt(1)] public bool IsAccountWhisper { get; set; }

    #region Dependency Injection

    [NotOnWire] private readonly IServiceProvider serviceProvider;

    public ClientChatWhisper(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    #endregion

    // The generated Read builds each format itself, so it needs no service provider.
    public ClientChatWhisper()
    {
    }

    public void Read(GamePacketReader reader)
    {
        ToName = reader.ReadWideString();
        ToRealmName = reader.ReadWideString();

        Message = reader.ReadWideString();

        byte formatCount = reader.ReadByte(5u);
        for (int i = 0; i < formatCount; i++)
        {
            ChatClientFormat format = serviceProvider.GetService<ChatClientFormat>();
            format.Read(reader);
            Formats.Add(format);
        }

        ChatMessageId = reader.ReadUShort();
        IsAccountWhisper = reader.ReadBit();
    }
}
