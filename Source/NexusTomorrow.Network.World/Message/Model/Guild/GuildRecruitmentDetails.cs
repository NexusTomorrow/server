using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server076A, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentDetails
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [WideString] public string Unknown1 { get; set; } = "";
    [Float32] public float Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    public GuildRecruitmentDemands Unknown5 { get; set; } = new();
}
