using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Renames a residence: the request Residence:RenameProperty sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client0529, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingRenameProperty
{
    public RealmScopedId Unknown0 { get; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
