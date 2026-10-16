using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client016A, Direction.ClientToServer)]
public sealed partial class Client016A
{
    [UInt(6)] public byte Unknown0 { get; private set; }
    [UInt(4)] public byte Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(3)] public byte Unknown3 { get; private set; }
}
