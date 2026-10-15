using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathChangeRequest)]
[WireMessage(GameMessageOpcode.ClientPathChangeRequest, Direction.ClientToServer)]
public sealed partial class ClientPathChangeRequest : IReadable
{
    [UInt(3)] public Path Path { get; private set; }
    [UInt(1)] public bool OnCooldown { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Path = reader.ReadEnum<Path>(3);
        OnCooldown = reader.ReadBit();
    }
}
