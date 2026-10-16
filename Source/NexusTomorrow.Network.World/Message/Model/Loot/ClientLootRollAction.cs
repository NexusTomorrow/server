using NexusTomorrow.Game.Static.Loot;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Client015D, Direction.ClientToServer)]
public sealed partial class Client015D
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(2)] public byte Unknown2 { get; private set; }
}
