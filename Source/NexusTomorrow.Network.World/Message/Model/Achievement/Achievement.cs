using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireStruct]
public sealed partial class Achievement : IWritable
{
    // TODO: Determine the data types for the Data0/Data1 fields for each achievement. This is based on the achievementTypeId

    [UInt(15)] public ushort AchievementId { get; set; }
    [UInt(32)] public uint Data0 { get; set; } // simply counted type: uint number
                                               // checklist type: flags for achievement objectives with index of 0 to 31
    [UInt(32)] public uint Data1 { get; set; } // checklist type: flags for achievement objectives with index of 32 to 63
    [UInt(64)] public ulong DateCompleted { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AchievementId, 15u);
        writer.Write(Data0);
        writer.Write(Data1);
        writer.Write(DateCompleted);
    }
}
