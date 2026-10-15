using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GalacticArchive;

// Sent when following GalacticArchiveLink or interacting with creatures that have archiveArticleIdInteractUnlock
[Message(GameMessageOpcode.ClientGalacticArchiveUnlock)]
[WireMessage(GameMessageOpcode.ClientGalacticArchiveUnlock, Direction.ClientToServer)]
public sealed partial class ClientGalacticArchiveUnlock : IReadable
{
    [UInt(32)] public uint ArchiveArticleId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ArchiveArticleId = reader.ReadUInt();
    }
}
