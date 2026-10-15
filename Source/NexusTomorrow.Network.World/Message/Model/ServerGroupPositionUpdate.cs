using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupPositionUpdate)]
[WireMessage(GameMessageOpcode.ServerGroupPositionUpdate, Direction.ServerToClient)]
public sealed partial class ServerGroupPositionUpdate : IWritable
{
    public class UnknownStruct0
    {
        public Identity Identity { get; set; }
        public Position Position { get; set; }
        public uint WorldZoneId { get; set; }
        public uint Flags { get; set; } = 0; // bInCombatPvp = 1, bIInCombatPve = 2, InCombat = 3
    }

    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(15)] public uint WorldId { get; set; }

    // Each member's update goes on the wire as four columns sharing one count: every Identity, every Position, every WorldZoneId, then every Flags.
    [UInt(32), Count] public uint UpdateCount { get; set; }
    [Array(CountFrom = nameof(UpdateCount))] public List<Identity> Identities { get; set; } = [];
    [Array(CountFrom = nameof(UpdateCount))] public List<Position> Positions { get; set; } = [];
    [Array(CountFrom = nameof(UpdateCount)), UInt(32)] public List<uint> WorldZoneIds { get; set; } = [];
    [Array(CountFrom = nameof(UpdateCount)), UInt(32)] public List<uint> Flags { get; set; } = [];
    [NotOnWire] public List<UnknownStruct0> Updates { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        writer.Write(WorldId, 15);

        writer.Write((uint)Updates.Count);
        Updates.ForEach(update => update.Identity.Write(writer));
        Updates.ForEach(update => update.Position.Write(writer));
        Updates.ForEach(update => writer.Write(update.WorldZoneId));
        Updates.ForEach(update => writer.Write(update.Flags));
    }
}
