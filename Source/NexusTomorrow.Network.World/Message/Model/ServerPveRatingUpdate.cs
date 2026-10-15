using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Very similar to ServerPvpRatingUpdate, but not sure what goes in it
// This triggers the lua event PveRatingUpdated but there are no uses of it in Carbine's lua code
[Message(GameMessageOpcode.ServerPveRatingUpdate)]
[WireMessage(GameMessageOpcode.ServerPveRatingUpdate, Direction.ServerToClient)]
public sealed partial class ServerPveRatingUpdate : IWritable
{
    [WireStruct]
    public sealed partial class PveRating : IWritable
    {
        [UInt(8)] public uint Category { get; set; } // similar to ServerPvpRatingUpdate, not sure what the categories are
        [UInt(3)] public MatchingGameRatingType Type { get; set; }
        [UInt(32)] public uint Unknown1 { get; set; }
        [UInt(32)] public uint Unknown2 { get; set; }
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(32)] public uint Unknown4 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Category, 8u);
            writer.Write(Type, 3u);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
        }
    }

    [Array(CountBits = 32)] public List<PveRating> PveRatings { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PveRatings.Count);
        foreach (PveRating rating in PveRatings)
        {
            rating.Write(writer);
        }
    }
}
