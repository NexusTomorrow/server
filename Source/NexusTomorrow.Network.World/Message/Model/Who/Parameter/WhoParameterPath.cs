using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter
{
    public class WhoParameterPath : IWhoParameterData
    {
        public Path PathId { get; private set; }

        public void Read(GamePacketReader reader)
        {
            PathId = reader.ReadEnum<Path>(3u);
        }
    }
}
