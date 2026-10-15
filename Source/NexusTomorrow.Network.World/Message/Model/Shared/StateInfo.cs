using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared
{
    public class StateInfo : IWritable
    {
        public PvpGameState State { get; set; }
        public uint TimeElapsed { get; set; } // in milliseconds

        public void Write(GamePacketWriter writer)
        {
            writer.Write(State, 3u);
            writer.Write(TimeElapsed);
        }
    }
}
