using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

// Only works when client is launched with gmode = China
[Message(GameMessageOpcode.ServerHealthyGamingUpdate)]
[WireMessage(GameMessageOpcode.ServerHealthyGamingUpdate, Direction.ServerToClient)]
public sealed partial class ServerHealthyGamingUpdate : IWritable
{
    [UInt(1)] public bool Enabled { get; set; }
    [UInt(2)] public HealthyGamingStatus Status { get; set; }
    [UInt(64)] public ulong GamingStartTime { get; set; } // Win32 FILETIME
    [UInt(32)] public uint AccumulatedOnlineTime { get; set; } // in hours
    [UInt(32)] public uint Unknown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Enabled);
        writer.Write(Status, 2u);
        writer.Write(GamingStartTime);
        writer.Write(AccumulatedOnlineTime);
        writer.Write(Unknown);
    }
}
