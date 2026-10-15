using NexusTomorrow.Game.Static.Chat;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[Message(GameMessageOpcode.ServerChat)]
[WireMessage(GameMessageOpcode.ServerChat, Direction.ServerToClient)]
public sealed partial class ServerChat : IWritable
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

    public void Write(GamePacketWriter writer)
    {
        Channel.Write(writer);

        writer.Write(GM);
        writer.Write(Self);
        writer.Write(AutoResponse);

        From.Write(writer);
        writer.WriteStringWide(FromName);
        writer.WriteStringWide(FromRealm);

        writer.Write(PresenceState, 3u);

        writer.WriteStringWide(Text);
        writer.Write(Formats.Count, 5u);
        Formats.ForEach(f => f.Write(writer));

        writer.Write(CrossFaction);
        writer.Write(ChatMessageId, 16u);

        writer.Write(UnitId, 32u);
        writer.Write(PremiumTier);
    }
}
