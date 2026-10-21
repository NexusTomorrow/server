using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

/// <summary>
/// Answers a shared quest: the request the script calls Quest:AcceptShare and Quest:RejectShare send.
/// </summary>
/// <remarks>
/// Neither call names the quest.
/// </remarks>
[WireMessage(GameMessageOpcode.Client045E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class AcceptSharedQuest
{
    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    [UInt(15)] public ushort Unknown0 { get; private set; }

    /// <summary>
    /// True from Quest:AcceptShare, false from Quest:RejectShare.
    /// </summary>
    [UInt(1)] public bool Accepted { get; private set; }
}
