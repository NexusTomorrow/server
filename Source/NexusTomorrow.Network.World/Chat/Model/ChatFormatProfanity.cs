using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.Profanity)]
public sealed partial class ChatFormatProfanity : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.Profanity;
    [UInt(14)] public uint RandomTextSeed { get; set; } // using current program time in milliseconds
}
