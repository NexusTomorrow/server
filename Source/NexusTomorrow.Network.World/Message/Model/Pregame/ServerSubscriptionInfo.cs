using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerSubscriptionInfo)]
[WireMessage(GameMessageOpcode.ServerSubscriptionInfo, Direction.ServerToClient)]
public sealed partial class ServerSubscriptionInfo : IWritable
{
    [Flags]
    public enum RealmTransferFlags
    {
        FreeRealmTransfer = 0x1,
    }

    [UInt(32)] public RealmTransferFlags Flags { get; set; }
    [UInt(1)] public bool SubscriptionExpired { get; set; }
    [Float32] public float GameTimeHoursRemaining { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Flags, 32u);
        writer.Write(SubscriptionExpired);
        writer.Write(GameTimeHoursRemaining);
    }
}
