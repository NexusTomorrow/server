using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ClientChat)]
[WireMessage(GameMessageOpcode.ClientChat, Direction.ClientToServer)]
public sealed partial class ClientChat : IReadable
{
    public Channel Channel { get; private set; }
    [WideString] public string Message { get; private set; }
    [Array(CountBits = 5)] public List<ChatClientFormat> Formats { get; } = [];
    [UInt(16)] public ushort ChatMessageId { get; private set; } // incremented by the client

    #region Dependency Injection

    [NotOnWire] private readonly IServiceProvider serviceProvider;

    public ClientChat(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    #endregion

    // The generated Read builds each format itself, so it needs no service provider.
    public ClientChat()
    {
    }

    public void Read(GamePacketReader reader)
    {
        Channel = new Channel();
        Channel.Read(reader);
        Message = reader.ReadWideString();

        byte formatCount = reader.ReadByte(5u);
        for (int i = 0; i < formatCount; i++)
        {
            ChatClientFormat format = serviceProvider.GetService<ChatClientFormat>();
            format.Read(reader);
            Formats.Add(format);
        }

        ChatMessageId = reader.ReadUShort();
    }
}
