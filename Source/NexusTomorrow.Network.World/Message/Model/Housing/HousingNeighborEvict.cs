using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Evicts a neighbor: the request HousingLib.NeighborEvict sends, among others.
/// </summary>
[WireMessage(GameMessageOpcode.Client0515, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingNeighborEvict
{
    /// <summary>
    /// The neighbor, by character id; zeros when the client names the neighbor instead.
    /// </summary>
    public RealmScopedId PlayerId { get; private set; } = new();

    [WideString] public string Unknown1 { get; private set; } = "";
}
