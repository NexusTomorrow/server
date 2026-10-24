using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.Message010B, Direction.ServerToClient)]
public sealed partial class HazardMaxModifiers
{
    [Array(CountBits = 32)] public List<HazardMaxModifier> Unknown0 { get; set; } = [];
    [Array(CountBits = 32)] public List<HazardMaxModifier> Unknown1 { get; set; } = [];
}
