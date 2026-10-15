using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerSpellUpdate, Direction.ServerToClient)]
public sealed partial class ServerSpellUpdate
{
    [UInt(18)] public uint Spell4BaseId { get; set; }
    [UInt(4)] public byte TierIndex { get; set; }
    [UInt(3)] public byte SpecIndex { get; set; }
    [UInt(1)] public bool Activated { get; set; }
}
