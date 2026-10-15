using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerGroupPositionUpdate, Direction.ServerToClient)]
public sealed partial class ServerGroupPositionUpdate
{
    [UInt(64)] public ulong GroupId { get; set; }
    [UInt(15)] public uint WorldId { get; set; }

    // Each member's update goes on the wire as four columns sharing one count: every Identity, every Position, every WorldZoneId, then every Flags.
    [UInt(32), Count] public uint UpdateCount { get; set; }
    [Array(CountFrom = nameof(UpdateCount))] public List<Identity> Identities { get; set; } = [];
    [Array(CountFrom = nameof(UpdateCount))] public List<Position> Positions { get; set; } = [];
    [Array(CountFrom = nameof(UpdateCount)), UInt(32)] public List<uint> WorldZoneIds { get; set; } = [];
    [Array(CountFrom = nameof(UpdateCount)), UInt(32)] public List<uint> Flags { get; set; } = [];
}
