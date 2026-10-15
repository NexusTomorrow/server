using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

// The legacy writer needs an OpponentPlayer or OpponentCreature for both Killer and Victim

[Message(GameMessageOpcode.ServerMatchingPvpKillNotification)]
[WireMessage(GameMessageOpcode.ServerMatchingPvpKillNotification, Direction.ServerToClient)]
public sealed partial class ServerMatchingPvpKillNotification : IWritable
{
    /// <summary>
    /// A killer or victim, a player or a creature as the <see cref="OpponentType"/> before it says.
    /// </summary>
    public interface IOpponent : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(OpponentType.Player)]
    public sealed partial class OpponentPlayer : IWritable, IOpponent
    {
        public Identity Identity { get; set; } = new();
        [UInt(5)] public Class Class { get; set; }

        public void Write(GamePacketWriter writer)
        {
            Identity.Write(writer);
            writer.Write(Class, 5u);
        }
    }

    [WireStruct]
    [UnionCase(OpponentType.Creature)]
    public sealed partial class OpponentCreature : IWritable, IOpponent
    {
        [UInt(32)] public uint UnitId { get; set; }
        [UInt(18)] public uint Creature2Id { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(Creature2Id, 18u);
        }
    }

    [UInt(2)] public OpponentType KillerType { get; set; }
    [Union(nameof(KillerType))] public IOpponent Killer { get; set; } = new OpponentPlayer();
    [NotOnWire] public OpponentPlayer KillerPlayer { get; set; }
    [NotOnWire] public OpponentCreature KillerCreature { get; set; }

    [UInt(2)] public OpponentType VictimType { get; set; }
    [Union(nameof(VictimType))] public IOpponent Victim { get; set; } = new OpponentPlayer();
    [NotOnWire] public OpponentPlayer VictimPlayer { get; set; }
    [NotOnWire] public OpponentCreature VictimCreature { get; set; }

    [UInt(2)] public MatchTeam VictimTeam { get; set; }
    [UInt(3)] public PvpDeathReason DeathReason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        if ((KillerPlayer == null && KillerCreature == null) || (VictimPlayer == null && VictimCreature == null))
        {
            throw new InvalidOperationException("Both a Killer and Victim must be created.");
        }

        writer.Write(KillerType, 2u);
        if (KillerType == OpponentType.Player)
        {
            KillerPlayer.Write(writer);
        }
        else if (KillerType == OpponentType.Creature)
        {
            KillerCreature.Write(writer);
        }

        writer.Write(VictimType, 2u);
        if (VictimType == OpponentType.Player)
        {
            VictimPlayer.Write(writer);
        }
        else if (VictimType == OpponentType.Creature)
        {
            VictimCreature.Write(writer);
        }

        writer.Write(VictimTeam, 2u);
        writer.Write(DeathReason, 3u);
    }
}
