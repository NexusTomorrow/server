using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientGuildRegister, Direction.ClientToServer)]
public sealed partial class ClientGuildRegister
{
    [UInt(32)] public uint InteractionUnitId { get; private set; }
    [UInt(4)] public GuildType GuildType { get; private set; }
    [WideString] public string GuildName { get; private set; }
    [WideString] public string MasterTitle { get; private set; }
    [WideString] public string CouncilTitle { get; private set; }
    [WideString] public string MemberTitle { get; private set; }
    public GuildStandard GuildStandard { get; private set; } = new();
    [UInt(1)] public bool AlternateCost { get; private set; }
}
