using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerSpellAbilityCharges, Direction.ServerToClient)]
public sealed partial class ServerSpellAbilityCharges
{
    [UInt(18)] public uint SpellId { get; set; }
    [UInt(32)] public uint AbilityChargeCount { get; set; }
}

