using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Client0249, Direction.ClientToServer)]
public sealed partial class CombatLogTypesSet
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
