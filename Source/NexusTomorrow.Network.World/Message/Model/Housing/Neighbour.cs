using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class Neighbour : IWritable
{
    [UInt(64)] public ulong NeighbourId { get; set; }
    [UInt(64)] public ulong Unused { get; set; } // is also a NeighbourId but not used by client, roommate maybe?
    public Identity PlayerIdentity { get; set; }
    [UInt(32)] public uint Permissions { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(NeighbourId);
        writer.Write(Unused);
        PlayerIdentity.Write(writer);
        writer.Write(Permissions);
    }
}
