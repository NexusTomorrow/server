using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune
{
    // Sent to the storefront server when the player has opened the Fortunes screen
    // Sent at the same time as ClientFortunesNotifyGame
    [Message(GameMessageOpcode.ClientFortuneNotifyStorefront)]
    public class ClientFortuneNotifyStorefront : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // zero byte message
        }
    }
}
