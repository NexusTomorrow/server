using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.Client094F, Direction.ClientToServer)]
public sealed partial class Client094F
{
    public RealmScopedId Unknown0 { get; private set; } = new RealmScopedId();
    [UInt(18)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
