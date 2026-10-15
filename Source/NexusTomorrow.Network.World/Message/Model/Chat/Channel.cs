using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireStruct]
public sealed partial class Channel
{
    [UInt(14)] public ChatChannelType ChatChannelId { get; set; }
    [UInt(64)] public ulong ChatId { get; set; }
}
