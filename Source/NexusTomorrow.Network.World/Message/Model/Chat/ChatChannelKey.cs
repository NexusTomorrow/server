using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireStruct]
public sealed partial class ChatChannelKey
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}
