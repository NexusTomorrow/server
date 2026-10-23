using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client009D, Direction.ClientToServer)]
public sealed partial class ObjectiveAbilityCast
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(18)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    public Client0096Part0 Unknown3 { get; private set; } = new Client0096Part0();
}
