using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingCrateAllDecor, Direction.ClientToServer)]
public sealed partial class ClientHousingCrateAllDecor
{
    public Identity TargetResidence { get; } = new();
}
