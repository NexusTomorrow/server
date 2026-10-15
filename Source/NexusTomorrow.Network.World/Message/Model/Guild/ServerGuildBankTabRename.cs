using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Sends all bank tab names.
[Message(GameMessageOpcode.ServerGuildBankTabRename)]
[WireMessage(GameMessageOpcode.ServerGuildBankTabRename, Direction.ServerToClient)]
public sealed partial class ServerGuildBankTabRename : IWritable
{
    public Identity GuildIdentity { get; set; }
    [FixedArray(10), WideString] public string[] BankTabNames { get; set; } = new string[10];

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        foreach (string tabName in BankTabNames)
        {
            writer.WriteStringWide(tabName);
        }
    }
}
