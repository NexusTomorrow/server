using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GalacticArchive;

[WireMessage(GameMessageOpcode.ServerGalacticArchiveUpdate, Direction.ServerToClient)]
public sealed partial class ServerGalacticArchiveUpdate
{
    [UInt(14)] public uint ArchiveArticleId { get; set; }
    [UInt(32)] public uint UnlockedFlags { get; set; } // Corresponds to the archiveEntry index not archiveEntryId, see archiveArticle tbl
    [UInt(32)] public uint ViewedFlags { get; set; } // Corresponds to the archiveEntry index not archiveEntryId, see archiveArticle tbl
}
