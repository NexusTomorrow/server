using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// An id qualified by the realm it belongs to.
/// </summary>
[WireStruct]
public sealed partial class RealmScopedId
{
    /// <summary>
    /// The realm the id belongs to.
    /// </summary>
    [UInt(14)] public ushort RealmId { get; set; }

    /// <summary>
    /// The id within its realm. What it identifies, a character or a residence, depends on the member that holds it.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    public override string ToString()
    {
        return $"{RealmId}:{Id}";
    }
}
