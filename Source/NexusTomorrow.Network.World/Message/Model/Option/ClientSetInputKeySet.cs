using NexusTomorrow.Game.Static.Option;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.ClientSetInputKeySet, Direction.ClientToServer)]
public sealed partial class ClientSetInputKeySet
{
    [UInt(32)] public InputSets InputKeySetEnum { get; private set; }
}
