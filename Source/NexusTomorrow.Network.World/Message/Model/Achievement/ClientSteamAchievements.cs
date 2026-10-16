using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.Client0823, Direction.ClientToServer)]
public sealed partial class Client0823
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [Blob(32)] public byte[] Unknown1 { get; private set; } = [];
}
