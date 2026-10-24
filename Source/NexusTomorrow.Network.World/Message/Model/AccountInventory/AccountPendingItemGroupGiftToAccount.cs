using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Client03F1, Direction.ClientToServer)]
public sealed partial class AccountPendingItemGroupGiftToAccount
{
    [WideString] public string Unknown0 { get; private set; } = "";
    [UInt(64)] public ulong Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    public RealmScopedId Unknown3 { get; private set; } = new();
}
