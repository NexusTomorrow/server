using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server077D, Direction.ServerToClient)]
public sealed partial class MissileRemoved
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
