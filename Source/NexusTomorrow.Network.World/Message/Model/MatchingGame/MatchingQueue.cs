using System.Diagnostics.CodeAnalysis;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Client05EF, Direction.ClientToServer)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "The request joins the matching queue; the type is not a collection.")]
public sealed partial class MatchingQueue
{
    public Client05EFPart0 Unknown0 { get; private set; } = new();
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
