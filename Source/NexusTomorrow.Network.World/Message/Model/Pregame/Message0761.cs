using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Message0761, Direction.ServerToClient)]
public sealed partial class Message0761
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Message0761Part0> Unknown1 { get; set; } = [];
    [Array(CountBits = 32)] public List<Message0761Part2> Unknown2 { get; set; } = [];
}
