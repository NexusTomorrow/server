using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.ServerChat, Direction.ServerToClient)]
public sealed partial class ServerChat
{
    public Channel Channel { get; set; }
    [UInt(1)] public bool GM { get; set; }
    [UInt(1)] public bool Self { get; set; }
    [UInt(1)] public bool AutoResponse { get; set; }
    public Identity From { get; set; }
    [WideString] public string FromName { get; set; }
    [WideString] public string FromRealm { get; set; }
    [UInt(3)] public AccountPresenceState PresenceState { get; set; }

    [WideString] public string Text { get; set; }
    [Array(CountBits = 5)] public List<ChatFormat> Formats { get; set; } = [];
    [UInt(1)] public bool CrossFaction { get; set; }
    [UInt(16)] public ushort ChatMessageId { get; set; } = 0; // always 0

    [UInt(32)] public ulong UnitId { get; set; }
    [UInt(8)] public byte PremiumTier { get; set; }
}
