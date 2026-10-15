using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Game.Static.Chat;

namespace NexusTomorrow.Network.World.Chat
{
    public class ChatFormatModelFactory : IChatFormatModelFactory
    {
        #region Dependency Injection

        private readonly IServiceProvider serviceProvider;

        public ChatFormatModelFactory(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion

        /// <summary>
        /// Returns a new <see cref="IChatFormatModel"/> model for supplied <see cref="ChatFormatType"/> type.
        /// </summary>
        public IChatFormatModel NewChatFormatModel(ChatFormatType type)
        {
            return serviceProvider.GetKeyedService<IChatFormatModel>(type);
        }
    }
}
