using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRenameProperty)]
public class ClientHousingRenameProperty : IReadable
{
    public Identity TargetResidence { get; } = new();
    public string Name { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TargetResidence.Read(reader);
        Name = reader.ReadWideString();
    }
}
