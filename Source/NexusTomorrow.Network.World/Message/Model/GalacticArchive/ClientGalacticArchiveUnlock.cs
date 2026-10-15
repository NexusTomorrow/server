using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GalacticArchive;

// Sent when following GalacticArchiveLink or interacting with creatures that have archiveArticleIdInteractUnlock
[WireMessage(GameMessageOpcode.ClientGalacticArchiveUnlock, Direction.ClientToServer)]
public sealed partial class ClientGalacticArchiveUnlock
{
    [UInt(32)] public uint ArchiveArticleId { get; private set; }
}
