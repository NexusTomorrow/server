using NexusTomorrow.Game.Static.Chat;

namespace NexusTomorrow.Network.World.Chat;

public interface IChatFormatModelFactory
{
    /// <summary>
    /// Returns a new <see cref="IChatFormatModel"/> model for supplied <see cref="ChatFormatType"/> type.
    /// </summary>
    IChatFormatModel NewChatFormatModel(ChatFormatType type);
}
