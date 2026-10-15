using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    // Fires whenever a player successfully withdraws money or items from the guild bank.  This only fires for the player who performed the withdraw action.
    [Message(GameMessageOpcode.ServerGuildBankWithdraw)]
    public class ServerGuildBankWithdraw : IWritable
    {
        public Identity GuildIdentity { get; private set; }
        public GuildWithdrawlInfo WithdrawlInfo { get; private set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            WithdrawlInfo.Write(writer);
        }
    }
}
