using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireStruct]
public sealed partial class Costume : IWritable
{
    public const byte MaxCostumeItems = 7;

    [UInt(32)] public uint Index { get; set; } // zero indexed, for CostumeType.Personal the limit is 12, for CostumeType.Mannequin the limit is 5
    [UInt(32)] public uint VisibilityMask { get; set; } // Bitmask representing which costume slots are visible, uses CostumeItemSlot as bit index
    [UInt(2)] public CostumeType Type { get; set; }
    [FixedArray(7), UInt(32)] public uint[] Item2Ids { get; set; } = new uint[7]; // in order of CostumeItemSlot enum
    [FixedArray(7), UInt(32)] public uint[] DyeData { get; set; } = new uint[7]; // in order of CostumeItemSlot enum

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Index);
        writer.Write(VisibilityMask);
        writer.Write(Type, 2u);

        for (int i = 0; i < 7; i++)
            writer.Write(Item2Ids[i]);
        for (int i = 0; i < 7; i++)
            writer.Write(DyeData[i]);
    }
}
