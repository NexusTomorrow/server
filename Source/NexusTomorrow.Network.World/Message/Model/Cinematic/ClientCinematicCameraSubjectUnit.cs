using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ClientCinematicCameraSubjectUnit, Direction.ClientToServer)]
public sealed partial class ClientCinematicCameraSubjectUnit
{
    [UInt(32)] public uint UnitId { get; private set; }
}
