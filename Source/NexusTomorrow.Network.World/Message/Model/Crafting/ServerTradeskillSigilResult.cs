using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

/// <summary>
/// The result of a tradeskill sigil; the client raises TradeSkillSigilResult with it.
/// </summary>
[WireMessage(GameMessageOpcode.Server085D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerTradeskillSigilResult
{
    /// <summary>
    /// The tradeskill sigil result, which the client raises as its TradeSkillSigilResult event.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte Result { get; set; }
}
