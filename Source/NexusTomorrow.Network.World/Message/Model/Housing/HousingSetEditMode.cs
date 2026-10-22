using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client053C, Direction.ClientToServer)]
public sealed partial class HousingSetEditMode
{
    public RealmScopedId Unknown0 { get; } = new();
    [UInt(1)] public bool Unknown1 { get; private set; }
}
