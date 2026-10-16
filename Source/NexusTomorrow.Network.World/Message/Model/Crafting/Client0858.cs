using NexusTomorrow.Game.Static.Crafting;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Client0858, Direction.ClientToServer)]
public sealed partial class Client0858
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
