using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingAddItemToCrate, Direction.ClientToServer)]
public sealed partial class ClientHousingAddItemToCrate
{
    [UInt(64)] public ulong ItemGuid { get; private set; }
}
