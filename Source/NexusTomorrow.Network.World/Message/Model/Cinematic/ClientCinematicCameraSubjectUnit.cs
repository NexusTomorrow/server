using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ClientCinematicCameraSubjectUnit)]
public class ClientCinematicCameraSubjectUnit : IReadable
{
    public uint UnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        UnitId = reader.ReadUInt();
    }
}
