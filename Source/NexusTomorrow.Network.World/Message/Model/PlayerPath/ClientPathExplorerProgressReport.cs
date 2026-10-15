using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathExplorerProgressReport)]
[WireMessage(GameMessageOpcode.ClientPathExplorerProgressReport, Direction.ClientToServer)]
public sealed partial class ClientPathExplorerProgressReport : IReadable
{
    [UInt(15)] public uint PathMissionId { get; private set; }
    [UInt(32)] public uint ExplorerNodeIndex { get; private set; } // Reports when player unit has reached radius threshold around WorldLocation.

    public void Read(GamePacketReader reader)
    {
        PathMissionId = reader.ReadUInt(15);
        ExplorerNodeIndex = reader.ReadUInt();
    }
}
