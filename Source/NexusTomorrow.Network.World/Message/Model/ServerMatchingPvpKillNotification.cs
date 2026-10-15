using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingPvpKillNotification, Direction.ServerToClient)]
public sealed partial class ServerMatchingPvpKillNotification
{
    /// <summary>
    /// A killer or victim, a player or a creature as the <see cref="OpponentType"/> before it says.
    /// </summary>
    public interface IOpponent : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(OpponentType.Player)]
    public sealed partial class OpponentPlayer : IOpponent
    {
        public Identity Identity { get; set; } = new();
        [UInt(5)] public Class Class { get; set; }
    }

    [WireStruct]
    [UnionCase(OpponentType.Creature)]
    public sealed partial class OpponentCreature : IOpponent
    {
        [UInt(32)] public uint UnitId { get; set; }
        [UInt(18)] public uint Creature2Id { get; set; }
    }

    [UInt(2)] public OpponentType KillerType { get; set; }
    [Union(nameof(KillerType))] public IOpponent Killer { get; set; } = new OpponentPlayer();

    [UInt(2)] public OpponentType VictimType { get; set; }
    [Union(nameof(VictimType))] public IOpponent Victim { get; set; } = new OpponentPlayer();

    [UInt(2)] public MatchTeam VictimTeam { get; set; }
    [UInt(3)] public PvpDeathReason DeathReason { get; set; }
}
