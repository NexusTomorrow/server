using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Response to ClientGetRealmTransferDestinations (0x3EE)
// Triggers TransferDestinationRealmList lua event.
[Message(GameMessageOpcode.ServerTransferDestinationRealmList)]
public class ServerTransferDestinationRealmList : IWritable
{
    public class RealmTransferInfo : RealmInfo
    {
        public bool IsFree { get; set; }

        public override void Write(GamePacketWriter writer)
        {
            base.Write(writer);
            writer.Write(IsFree);
        }
    }

    public List<RealmTransferInfo> Realms { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Realms.Count);
        Realms.ForEach(s => s.Write(writer));
    }
}
