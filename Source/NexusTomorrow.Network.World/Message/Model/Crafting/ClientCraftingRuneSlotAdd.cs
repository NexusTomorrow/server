using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Client0859, Direction.ClientToServer)]
public sealed partial class Client0859
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
    [UInt(5)] public byte Unknown2 { get; private set; }
}
