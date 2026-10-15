using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.Alien)]
public sealed partial class ChatFormatAlien : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.Alien;
    [UInt(14)] public uint RandomTextSeed { get; set; }
}
