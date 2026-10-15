using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Sends all bank tab names.
[WireMessage(GameMessageOpcode.ServerGuildBankTabRename, Direction.ServerToClient)]
public sealed partial class ServerGuildBankTabRename
{
    public Identity GuildIdentity { get; set; }
    [FixedArray(10), WideString] public string[] BankTabNames { get; set; } = new string[10];
}
