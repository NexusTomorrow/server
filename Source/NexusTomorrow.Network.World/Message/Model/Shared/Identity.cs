using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Identity
{
    [UInt(14)] public ushort RealmId { get; set; }
    [UInt(64)] public ulong Id { get; set; }

    public override string ToString()
    {
        return $"{RealmId}:{Id}";
    }
}
