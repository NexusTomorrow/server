using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Client0720, Direction.ClientToServer)]
public sealed partial class CinematicProgress
{
    [UInt(8)] public byte Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
