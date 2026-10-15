using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Response to ClientGetRealmTransferDestinations (0x3EE)
// Triggers TransferDestinationRealmList lua event.
[Message(GameMessageOpcode.ServerTransferDestinationRealmList)]
[WireMessage(GameMessageOpcode.ServerTransferDestinationRealmList, Direction.ServerToClient)]
public sealed partial class ServerTransferDestinationRealmList : IWritable
{
    [WireStruct]
    public sealed partial class RealmTransferInfo : RealmInfo
    {
        [UInt(1)] public bool IsFree { get; set; }

        public override void Write(GamePacketWriter writer)
        {
            base.Write(writer);
            writer.Write(IsFree);
        }
    }

    [Array(CountBits = 32)] public List<RealmTransferInfo> Realms { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Realms.Count);
        Realms.ForEach(s => s.Write(writer));
    }
}
