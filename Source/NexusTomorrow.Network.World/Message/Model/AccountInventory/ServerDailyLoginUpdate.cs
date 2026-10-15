using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerDailyLoginUpdate)]
[WireMessage(GameMessageOpcode.ServerDailyLoginUpdate, Direction.ServerToClient)]
public sealed partial class ServerDailyLoginUpdate : IWritable
{
    [UInt(32)] public uint LoginDaysTotal { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint CurrentLoginDays { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint NextDailyRewardNotifyTime { get; set; } // relative time from now in milliseconds
    [Float32] public float NextLockboxKeyTimeInDays { get; set; }
    [UInt(3)] public PremiumLockboxKeyStatus PremiumKeyStatus { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(LoginDaysTotal);
        writer.Write(Unknown1);
        writer.Write(CurrentLoginDays);
        writer.Write(Unknown2);
        writer.Write(NextDailyRewardNotifyTime);
        writer.Write(NextLockboxKeyTimeInDays);
        writer.Write(PremiumKeyStatus, 3u);
    }
}
