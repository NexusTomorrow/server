using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client00B3, Direction.ClientToServer)]
public sealed partial class UnitActivate
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
