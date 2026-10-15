using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

// Only works when client is launched with gmode = China
[WireMessage(GameMessageOpcode.ServerHealthyGamingUpdate, Direction.ServerToClient)]
public sealed partial class ServerHealthyGamingUpdate
{
    [UInt(1)] public bool Enabled { get; set; }
    [UInt(2)] public HealthyGamingStatus Status { get; set; }
    [UInt(64)] public ulong GamingStartTime { get; set; } // Win32 FILETIME
    [UInt(32)] public uint AccumulatedOnlineTime { get; set; } // in hours
    [UInt(32)] public uint Unknown { get; set; }
}
