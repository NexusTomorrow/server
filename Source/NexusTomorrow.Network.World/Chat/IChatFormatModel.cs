using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Chat;

public interface IChatFormatModel : IWireStruct
{
    ChatFormatType Type { get; }
}
