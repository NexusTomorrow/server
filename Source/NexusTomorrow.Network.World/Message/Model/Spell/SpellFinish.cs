using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server07FE, Direction.ServerToClient)]

public sealed partial class SpellFinish
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
