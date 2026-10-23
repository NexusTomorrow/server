using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Client071F, Direction.ClientToServer)]
public sealed partial class CinematicCameraSubjectUnit
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
