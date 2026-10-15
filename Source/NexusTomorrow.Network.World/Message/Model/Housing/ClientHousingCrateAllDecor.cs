using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingCrateAllDecor)]
[WireMessage(GameMessageOpcode.ClientHousingCrateAllDecor, Direction.ClientToServer)]
public sealed partial class ClientHousingCrateAllDecor : IReadable
{
    public Identity TargetResidence { get; } = new();

    public void Read(GamePacketReader reader)
    {
        TargetResidence.Read(reader);
    }
}
