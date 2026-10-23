using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Asks to place a residence in a community: the request HousingLib.RequestCommunityPlacement sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client052B, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingRequestCommunityPlacement
{
    public RealmScopedId Unknown0 { get; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
}
