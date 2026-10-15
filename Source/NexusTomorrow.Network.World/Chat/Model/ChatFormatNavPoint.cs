using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Chat.Model;

[WireStruct]
[UnionCase(ChatFormatType.NavPoint)]
public sealed partial class ChatFormatNavPoint : IChatFormatModel
{
    [NotOnWire] public ChatFormatType Type => ChatFormatType.NavPoint;
    [UInt(14)] public ushort MapZoneId { get; set; }
    [Float32] public float X { get; set; }
    [Float32] public float Y { get; set; }
}
