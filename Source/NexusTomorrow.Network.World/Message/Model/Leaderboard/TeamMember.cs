using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireStruct]
public sealed partial class TeamMember : IWritable
{
    [WideString] public string Name { get; set; }
    [UInt(5)] public Class Class { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Name);
        writer.Write(Class, 5u);
    }
}
