using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.World.Chat;

namespace NexusTomorrow.Network.World.Message.Model.Chat
{
    public class ChatClientFormat : ChatFormat
    {
        #region Dependency Injection

        private readonly IChatFormatModelFactory chatFormatFactory;

        public ChatClientFormat(
            IChatFormatModelFactory chatFormatFactory)
        {
            this.chatFormatFactory = chatFormatFactory;
        }

        #endregion

        public void Read(GamePacketReader reader)
        {
            Type       = reader.ReadEnum<ChatFormatType>(4);
            StartIndex = reader.ReadUShort();
            StopIndex  = reader.ReadUShort();

            Model = chatFormatFactory.NewChatFormatModel(Type);
            if (Model == null)
                throw new NotImplementedException();

            Model.Read(reader);
        }
    }
}
