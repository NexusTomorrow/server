using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server0152, Direction.ServerToClient)]
public sealed partial class Server0152
{
    [UInt(7), Count] public byte Unknown0 { get; set; }
    [Array(CountFrom = nameof(Unknown0)), UInt(3)] public List<ushort> Unknown1 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown0)), UInt(3)] public List<byte> Unknown2 { get; set; } = [];
}
