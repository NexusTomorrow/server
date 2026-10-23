using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client0177, Direction.ClientToServer)]
public sealed partial class WeaponsSheathe
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
