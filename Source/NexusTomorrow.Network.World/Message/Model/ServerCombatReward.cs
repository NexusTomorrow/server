using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Used by <see cref="CurrencyManager"/>, and also used during Combat to provide Momentum Boosts.
/// </summary>
[Message(GameMessageOpcode.ServerCombatReward)]
[WireMessage(GameMessageOpcode.ServerCombatReward, Direction.ServerToClient)]
public sealed partial class ServerCombatReward : IWritable
{
    [UInt(5)] public byte Stat { get; set; }
    [UInt(64)] public ulong NewValue { get; set; }
    [UInt(32)] public uint CombatRewardId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Stat, 5);
        writer.Write(NewValue);
        writer.Write(CombatRewardId);
        writer.Write(TargetUnitId);
    }
}
