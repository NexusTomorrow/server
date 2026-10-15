using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who;

[WireMessage(GameMessageOpcode.ServerWhoResponse, Direction.ServerToClient)]
public sealed partial class ServerWhoResponse
{
    [WireStruct]
    public sealed partial class WhoPlayer
    {
        [WideString] public string Name { get; set; }
        [WideString] public string Realm { get; set; }
        [UInt(32)] public uint Level { get; set; }
        [UInt(5)] public Race Race { get; set; }
        [UInt(5)] public Class Class { get; set; }
        [UInt(2)] public Sex Sex { get; set; }
        [UInt(14)] public Faction Faction { get; set; }
        [UInt(3)] public Path Path { get; set; }
        [UInt(15)] public uint Zone { get; set; }
    }

    [Array(CountBits = 32)] public List<WhoPlayer> Players { get; set; }
    [UInt(2)] public WhoResult Result { get; set; }
}
