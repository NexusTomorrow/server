using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Client03F2, Direction.ClientToServer)]
public sealed partial class AccountPendingItemGroupGiftToCharacter
{
    [WideString] public string Unknown0 { get; private set; } = "";
    public RealmScopedId Unknown1 { get; private set; } = new();
}
