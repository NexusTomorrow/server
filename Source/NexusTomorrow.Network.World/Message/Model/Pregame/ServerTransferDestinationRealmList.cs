using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Response to ClientGetRealmTransferDestinations (0x3EE)
// Triggers TransferDestinationRealmList lua event.
[WireMessage(GameMessageOpcode.ServerTransferDestinationRealmList, Direction.ServerToClient)]
public sealed partial class ServerTransferDestinationRealmList
{
    [WireStruct]
    public sealed partial class RealmTransferInfo : RealmInfo
    {
        [UInt(1)] public bool IsFree { get; set; }
    }

    [Array(CountBits = 32)] public List<RealmTransferInfo> Realms { get; set; } = [];
}
