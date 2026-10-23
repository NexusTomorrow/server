using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server076D, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentRecruiters
{
    [WireStruct]
    public sealed partial class Change
    {
        [UInt(64)] public ulong Unknown0 { get; set; }
        [UInt(1)] public bool Unknown1 { get; set; }
        [UInt(32), Count] public uint Unknown2 { get; set; }
        [Array(CountFrom = nameof(Unknown2)), WideString] public List<string> Unknown3 { get; set; } = [];
        [Array(CountFrom = nameof(Unknown2)), UInt(1)] public List<bool> Unknown4 { get; set; } = [];
    }

    [Array(CountBits = 32)] public List<Change> Unknown0 { get; set; } = [];
}
