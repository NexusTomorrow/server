using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Chat;

public interface IChatFormatModel : IReadable, IWritable
{
    ChatFormatType Type { get; }
}
