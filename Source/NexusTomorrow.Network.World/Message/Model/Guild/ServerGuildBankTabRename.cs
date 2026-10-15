using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Sends all bank tab names.
[Message(GameMessageOpcode.ServerGuildBankTabRename)]
public class ServerGuildBankTabRename : IWritable
{
    public Identity GuildIdentity { get; set; }
    public string[] BankTabNames { get; set; } = new string[10];

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        foreach (string tabName in BankTabNames)
        {
            writer.WriteStringWide(tabName);
        }
    }
}
