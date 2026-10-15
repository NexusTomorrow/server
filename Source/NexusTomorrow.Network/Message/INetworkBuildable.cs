namespace NexusTomorrow.Network.Message
{
    public interface INetworkBuildable<out T>
    {
        T Build();
    }

    public interface INetworkBuildable
    {
        IWritable Build();
    }
}
