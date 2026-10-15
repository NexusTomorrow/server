using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
[UnionCase(WhoParameterType.Guild)]
public sealed partial class WhoParameterGuild : IWhoParameterData
{
    [WideString] public string GuildName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        GuildName = reader.ReadWideString();
    }
}
