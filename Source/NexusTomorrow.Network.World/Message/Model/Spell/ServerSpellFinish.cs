using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellFinish, Direction.ServerToClient)]

public sealed partial class ServerSpellFinish
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
}
