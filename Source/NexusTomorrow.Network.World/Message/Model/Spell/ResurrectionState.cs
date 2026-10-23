using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Server0100, Direction.ServerToClient)]
public sealed partial class ResurrectionState
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
}
