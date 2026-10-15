using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.Roleplay)]
public sealed partial class ChatFormatRoleplay : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.Roleplay;
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
