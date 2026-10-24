using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

/// <summary>
/// Asks for a random roll in a range, when the player types the /roll or /random chat command. The client sends the range only, and
/// sends nothing unless <see cref="Minimum"/> is at most <see cref="Maximum"/> and <see cref="Maximum"/> is at most 1,000,000. The
/// server draws the roll and answers with <see cref="RandomRollResult"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Client071B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class RandomRollRequest
{
    /// <summary>
    /// The client gives this value no meaning. It always sends realm 0 and id 0.
    /// </summary>
    [VerifiedMember] public RealmScopedId Unknown0 { get; } = new();

    /// <summary>
    /// The low end of the range: the first number when the player types two, and 1 when the player types one.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Minimum { get; private set; }

    /// <summary>
    /// The high end of the range: the second number when the player types two, and the one number when the player types one.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Maximum { get; private set; }

    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown3 { get; private set; }
}
