using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ClientGuildBankMoneyTransaction)]
    public class ClientGuildBankMoneyTransaction : IReadable
    {
        public Identity GuildIdentity { get; private set; } = new();
        public long Amount { get; private set; } // Negative for deposit, positive for withdraw

        public void Read(GamePacketReader reader)
        {
            GuildIdentity.Read(reader);
            Amount = reader.ReadLong();
        }
    }
}
