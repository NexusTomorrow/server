using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0196, Direction.ServerToClient)]
public sealed partial class Server0196
{
    public enum P2PTradeResult
    {
        ErrorInitiating = 0,
        PlayerLogOut = 1,
        PlayerCanceled = 2,
        PlayerAcceptedInvite = 3,
        PlayerDeclinedInvite = 4,
        MissingPlayer = 5,
        InitiatorCommitted = 6,
        InitiatorUnCommitted = 7,
        TargetCommitted = 8,
        TargetUnCommitted = 9,
        NothingToTrade = 10,
        FinishedSuccess = 11,
        DbFailedToInit = 12,
        DbFailed = 13,
        ErrorAddingItem = 14,
        ErrorRemovingItem = 15,
        TargetBusy = 16,
        TargetNotAllowedToTrade = 17,
        InviteFailedNoTrading = 18
    }

    [UInt(5)] public byte Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
