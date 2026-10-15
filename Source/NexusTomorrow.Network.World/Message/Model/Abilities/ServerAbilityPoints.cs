using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerAbilityPoints, Direction.ServerToClient)]
public sealed partial class ServerAbilityPoints
{
    [UInt(32)] public uint AbilityPoints { get; set; }
    [UInt(32)] public uint TotalAbilityPoints { get; set; }
}
