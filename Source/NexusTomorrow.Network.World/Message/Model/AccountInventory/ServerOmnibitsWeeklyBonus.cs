using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerOmnibitsWeeklyBonus)]
[WireMessage(GameMessageOpcode.ServerOmnibitsWeeklyBonus, Direction.ServerToClient)]
public sealed partial class ServerOmnibitsWeeklyBonus : IWritable
{
    [UInt(32)] public uint OmnibitsWeeklylBonusEarned { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(OmnibitsWeeklylBonusEarned);
    }
}
