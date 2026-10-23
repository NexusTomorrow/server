using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0497, Direction.ServerToClient)]
public sealed partial class Server0497
{
    [UInt(32), Count] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [Array(CountFrom = nameof(Unknown0))] public List<Server0493Part0> Unknown2 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0))] public List<Server0493Part5> Unknown3 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0))] public List<GuildBankWithdrawals> Unknown4 { get; set; } = [];
}
