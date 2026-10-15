using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

// Only works when client is launched with gmode = China
[Message(GameMessageOpcode.ServerHealthyGamingUpdate)]
public class ServerHealthyGamingUpdate : IWritable
{
    public bool Enabled { get; set; }
    public HealthyGamingStatus Status { get; set; }
    public ulong GamingStartTime { get; set; } // Win32 FILETIME
    public uint AccumulatedOnlineTime { get; set; } // in hours
    public uint Unknown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Enabled);
        writer.Write(Status, 2u);
        writer.Write(GamingStartTime);
        writer.Write(AccumulatedOnlineTime);
        writer.Write(Unknown);
    }
}
