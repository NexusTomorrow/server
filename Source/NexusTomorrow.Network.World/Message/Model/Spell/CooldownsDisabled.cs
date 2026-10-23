using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server080E, Direction.ServerToClient)]
public sealed partial class CooldownsDisabled
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
