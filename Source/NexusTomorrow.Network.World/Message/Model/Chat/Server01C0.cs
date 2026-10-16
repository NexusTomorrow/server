using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01C0, Direction.ServerToClient)]
public sealed partial class Server01C0
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(32), Count] public uint Unknown2 { get; set; }
    [Array(CountFrom = nameof(Unknown2)), WideString] public List<string> Unknown3 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown2)), UInt(32)] public List<uint> Unknown4 { get; set; } = [];
}
