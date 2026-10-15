using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[Message(GameMessageOpcode.ClientIncidentReport)]
[WireMessage(GameMessageOpcode.ClientIncidentReport, Direction.ClientToServer)]
public sealed partial class ClientIncidentReport : IReadable
{
    public Identity Identity { get; private set; } = new();
    [UInt(3)] public ReportPlayerReason Reason { get; private set; }
    [UInt(4)] public ReportPlayerSource Source { get; private set; }
    [WideString] public string Note { get; private set; }
    [UInt(64)] public ulong ObjectId { get; private set; } // mailId, guildInviteId, friendshipInviteId, friendAccountInviteId, unitId
    [Float32] public float DaysAgo { get; private set; }
    [UInt(1)] public bool PermanentIgnore { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Identity.Read(reader);
        Reason = reader.ReadEnum<ReportPlayerReason>(3u);
        Source = reader.ReadEnum<ReportPlayerSource>(4u);
        Note = reader.ReadWideString();
        ObjectId = reader.ReadULong();
        DaysAgo = reader.ReadSingle();
        PermanentIgnore = reader.ReadBit();
    }
}
