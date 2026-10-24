using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Tells the player that a group member asks for another player to be invited.
/// </summary>
/// <remarks>
/// The client keeps it as the pending request, which <see cref="GroupRequestAnswer"/> answers, and raises Group_Referral with the
/// referred player's name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server044F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupReferral
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    public RealmScopedId Unknown1 { get; set; } = new();

    /// <summary>
    /// The player referred, by name.
    /// </summary>
    [WideString] public string TargetName { get; set; } = "";
}
