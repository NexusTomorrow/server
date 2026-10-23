using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0533, Direction.ServerToClient)]
public sealed partial class HousingOwnership
{
    public RealmScopedId Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
