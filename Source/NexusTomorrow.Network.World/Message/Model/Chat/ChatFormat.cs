using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Chat;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireStruct]
public partial class ChatFormat
{
    [UInt(4)] public ChatFormatType Type { get; set; }
    [UInt(16)] public ushort StartIndex { get; set; }
    [UInt(16)] public ushort StopIndex { get; set; }
    [Union(nameof(Type))] public IChatFormatModel Model { get; set; }
}
