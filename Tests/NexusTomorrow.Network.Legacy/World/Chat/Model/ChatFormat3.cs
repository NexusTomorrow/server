using NexusTomorrow.Game.Static.Chat;

namespace NexusTomorrow.Network.World.Chat.Model;

public class ChatFormat3 : IChatFormatModel
{
    public ChatFormatType Type => ChatFormatType.Format3;
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
