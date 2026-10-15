using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GalacticArchive;

// Empties the set of galacticArchive state. Must send again with ServerGalacticArchiveUpdate.
[WireMessage(GameMessageOpcode.ServerGalacticArchiveRefresh, Direction.ServerToClient)]
public sealed partial class ServerGalacticArchiveRefresh
{
}
