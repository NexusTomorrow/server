using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ClientCinematicCameraSubjectUnit)]
[WireMessage(GameMessageOpcode.ClientCinematicCameraSubjectUnit, Direction.ClientToServer)]
public sealed partial class ClientCinematicCameraSubjectUnit : IReadable
{
    [UInt(32)] public uint UnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
    }
}
