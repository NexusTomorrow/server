using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingCommunityRename, Direction.ClientToServer)]
public sealed partial class ClientHousingCommunityRename
{
    public Identity TargetGuild { get; } = new();
    [WideString] public string Name { get; private set; }
    [UInt(1)] public bool UseServiceToken { get; private set; } // Uses ServiceToken
}
