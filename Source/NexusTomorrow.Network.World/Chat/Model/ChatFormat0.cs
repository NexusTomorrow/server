using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
public sealed partial class ChatFormat0 : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.Format0;
    [UInt(1)] public bool Unknown { get; set; }

    public void Read(GamePacketReader reader)
    {
        Unknown = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unknown);
    }
}
