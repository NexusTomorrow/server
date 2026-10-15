using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerWarPartyBossTokens)]
    public class ServerWarPartyBossTokens : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public List<WarPartyBossToken> Tokens { get; set; } = [];

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.Write(Tokens.Count);
            Tokens.ForEach(token => token.Write(writer));
        }
    }
}
