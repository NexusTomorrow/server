using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathUnlockRequest)]
[WireMessage(GameMessageOpcode.ClientPathUnlockRequest, Direction.ClientToServer)]
public sealed partial class ClientPathUnlockRequest : IReadable
{
    [UInt(3)] public Path Path { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Path = reader.ReadEnum<Path>(3);
    }
}
