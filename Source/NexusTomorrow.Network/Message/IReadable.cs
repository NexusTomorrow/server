namespace NexusTomorrow.Network.Message
{
    public interface IReadable
    {
        void Read(GamePacketReader reader);
    }
}
