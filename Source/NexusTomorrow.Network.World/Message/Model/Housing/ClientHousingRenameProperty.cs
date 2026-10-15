using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRenameProperty)]
[WireMessage(GameMessageOpcode.ClientHousingRenameProperty, Direction.ClientToServer)]
public sealed partial class ClientHousingRenameProperty : IReadable
{
    public Identity TargetResidence { get; } = new();
    [WideString] public string Name { get; private set; }

    public void Read(GamePacketReader reader)
    {
        TargetResidence.Read(reader);
        Name = reader.ReadWideString();
    }
}
