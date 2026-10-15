using NexusTomorrow.Game.Static.Support;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.ClientIncidentReport, Direction.ClientToServer)]
public sealed partial class ClientIncidentReport
{
    public Identity Identity { get; private set; } = new();
    [UInt(3)] public ReportPlayerReason Reason { get; private set; }
    [UInt(4)] public ReportPlayerSource Source { get; private set; }
    [WideString] public string Note { get; private set; }
    [UInt(64)] public ulong ObjectId { get; private set; } // mailId, guildInviteId, friendshipInviteId, friendAccountInviteId, unitId
    [Float32] public float DaysAgo { get; private set; }
    [UInt(1)] public bool PermanentIgnore { get; private set; }
}
