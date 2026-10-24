using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

[WireMessage(GameMessageOpcode.Server0469, Direction.ServerToClient)]
public sealed partial class GroupMemberPositions
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(15)] public uint Unknown1 { get; set; }

    [UInt(32), Count] public uint Unknown2 { get; set; }
    [Array(CountFrom = nameof(Unknown2))] public List<RealmScopedId> Unknown3 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown2))] public List<Client0096Part0> Unknown4 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown5 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown6 { get; set; } = [];
}
