using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingOwnership)]
[WireMessage(GameMessageOpcode.ServerHousingOwnership, Direction.ServerToClient)]
public sealed partial class ServerHousingOwnership : IWritable
{
    public Identity ResidenceIdentity { get; set; }
    [UInt(32)] public OwnershipType Ownership { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);
        writer.Write(Ownership, 32u);
    }
}
