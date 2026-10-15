using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientCastServiceToken, Direction.ClientToServer)]
public sealed partial class ClientCastServiceToken
{
    [UInt(18)] public uint Spell4Id { get; private set; }
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
}
