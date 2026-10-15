using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathExplorerPowerMapProgress)]
[WireMessage(GameMessageOpcode.ClientPathExplorerPowerMapProgress, Direction.ClientToServer)]
public sealed partial class ClientPathExplorerPowerMapProgress : IReadable
{
    [UInt(14)] public uint PathMissionId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PathMissionId = reader.ReadUInt(14);
    }
}
