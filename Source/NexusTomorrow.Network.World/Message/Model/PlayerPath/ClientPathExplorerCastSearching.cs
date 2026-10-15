using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientCastPathExplorerSearching, Direction.ClientToServer)]
public sealed partial class ClientPathExplorerCastSearching
{
    [UInt(32)] public uint ClientSpellCastUniqueID { get; private set; }
    [UInt(2)] public byte SearchRadiusBand { get; private set; } // casts 1 of 4 Searching spells depending on band
    [UInt(14)] public ushort PathExplorerScavengerClueId { get; private set; } // Relates to nearest ScavengerHunt WorldLocation
}
