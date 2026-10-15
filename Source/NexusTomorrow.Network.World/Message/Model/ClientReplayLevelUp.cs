using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientReplayLevelRequest)]
[WireMessage(GameMessageOpcode.ClientReplayLevelRequest, Direction.ClientToServer)]
public sealed partial class ClientReplayLevelUp : IReadable
{
    [UInt(32)] public uint Level { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Level = reader.ReadUInt();
    }
}
