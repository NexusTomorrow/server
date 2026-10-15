using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Chat;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireStruct]
public sealed partial class ChatClientFormat : ChatFormat
{
    #region Dependency Injection

    [NotOnWire] private readonly IChatFormatModelFactory chatFormatFactory;

    public ChatClientFormat(
        IChatFormatModelFactory chatFormatFactory)
    {
        this.chatFormatFactory = chatFormatFactory;
    }

    #endregion

    // The generated Read picks the model from the [UnionCase] arms, so it needs no factory.
    public ChatClientFormat()
    {
    }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<ChatFormatType>(4);
        StartIndex = reader.ReadUShort();
        StopIndex = reader.ReadUShort();

        Model = chatFormatFactory.NewChatFormatModel(Type);
        if (Model == null)
            throw new NotImplementedException();

        Model.Read(reader);
    }
}
