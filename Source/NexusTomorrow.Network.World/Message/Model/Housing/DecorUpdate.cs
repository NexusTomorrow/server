using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

public class DecorUpdate : IReadable
{
    public DecorInfo DecorInfo { get; internal set; } = new();
    public bool UseServiceToken { get; internal set; }

    public void Read(GamePacketReader reader)
    {
        DecorInfo.Read(reader);
    }
}
