using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client0180, Direction.ClientToServer)]
public sealed partial class Client0180
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
