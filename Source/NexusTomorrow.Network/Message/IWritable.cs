namespace NexusTomorrow.Network.Message
{
    public interface IWritable
    {
        void Write(GamePacketWriter writer);
    }
}
