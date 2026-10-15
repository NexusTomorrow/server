using NexusTomorrow.Game.Static.Chat;

namespace NexusTomorrow.Network.World.Chat.Model
{
    public class ChatFormatRoleplay : IChatFormatModel
    {
        public ChatFormatType Type => ChatFormatType.Roleplay;
        public bool Unknown { get; set; }

        public void Read(GamePacketReader reader)
        {
            Unknown = reader.ReadBit();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown);
        }
    }
}
