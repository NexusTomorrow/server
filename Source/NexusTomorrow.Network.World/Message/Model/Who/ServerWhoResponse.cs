using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Game.Static.Who;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who;

[Message(GameMessageOpcode.ServerWhoResponse)]
[WireMessage(GameMessageOpcode.ServerWhoResponse, Direction.ServerToClient)]
public sealed partial class ServerWhoResponse : IWritable
{
    [WireStruct]
    public sealed partial class WhoPlayer : IWritable
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

        public void Write(GamePacketWriter writer)
        {
            writer.WriteStringWide(Name);
            writer.WriteStringWide(Realm);
            writer.Write(Level, 32);
            writer.Write(Race, 5);
            writer.Write(Class, 5);
            writer.Write(Sex, 2);
            writer.Write(Faction, 14);
            writer.Write(Path, 3);
            writer.Write(Zone, 15);
        }
    }

    [Array(CountBits = 32)] public List<WhoPlayer> Players { get; set; }
    [UInt(2)] public WhoResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Players.Count, 32u);
        Players.ForEach((player) => player.Write(writer));
        writer.Write(Result, 2u);
    }
}
