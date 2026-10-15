using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public partial class StateInfo : IWritable
{
    [UInt(3)] public PvpGameState State { get; set; }
    [UInt(32)] public uint TimeElapsed { get; set; } // in milliseconds

    public void Write(GamePacketWriter writer)
    {
        writer.Write(State, 3u);
        writer.Write(TimeElapsed);
    }
}
