using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GalacticArchive;

// Sent when SetViewed function is called on an archiveEntry that is unlocked but unviewed
// Relies on bit flags sent in ServerGalacticArchiveUpdate to determine which archiveEntry was viewed for the given ArchiveArticleId
[Message(GameMessageOpcode.ClientGalacticArchiveViewed)]
[WireMessage(GameMessageOpcode.ClientGalacticArchiveViewed, Direction.ClientToServer)]
public sealed partial class ClientGalacticArchiveViewed : IReadable
{
    [UInt(32)] public uint ArchiveArticleId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ArchiveArticleId = reader.ReadUInt();
    }
}
