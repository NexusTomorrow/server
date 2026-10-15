using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingEditMode)]
[WireMessage(GameMessageOpcode.ClientHousingEditMode, Direction.ClientToServer)]
public sealed partial class ClientHousingEditMode : IReadable
{
    public Identity ResidenceIdentity { get; } = new(); // Can be a residence the player has rights to edit, a community residence, or war party plot
    [UInt(1)] public bool Enabled { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);
        Enabled = reader.ReadBit();
    }
}
