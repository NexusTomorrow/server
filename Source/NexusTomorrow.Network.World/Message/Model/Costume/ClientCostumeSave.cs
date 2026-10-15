using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ClientCostumeSave)]
[WireMessage(GameMessageOpcode.ClientCostumeSave, Direction.ClientToServer)]
public sealed partial class ClientCostumeSave : IReadable
{
    [WireStruct]
    public sealed partial class CostumeItem : IReadable
    {
        [UInt(18)] public uint Item2Id { get; private set; }
        [FixedArray(3), UInt(32)] public uint[] DyeColorRampIds { get; } = new uint[3];

        public void Read(GamePacketReader reader)
        {
            Item2Id = reader.ReadUInt(18u);
            for (int i = 0; i < 3; i++)
                DyeColorRampIds[i] = reader.ReadUInt();
        }
    }

    [SInt(32)] public int Index { get; private set; }
    [UInt(2)] public CostumeType Type { get; private set; }
    [UInt(64)] public ulong MannequinDecorId { get; private set; }
    [FixedArray(Costume.MaxCostumeItems)] public CostumeItem[] Items { get; } = new CostumeItem[Costume.MaxCostumeItems]; // in order of CostumeItemSlot enum
    [UInt(32)] public uint VisibilityMask { get; private set; }
    [UInt(1)] public bool UserServiceToken { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Index = reader.ReadInt();
        Type = reader.ReadEnum<CostumeType>(2u);
        MannequinDecorId = reader.ReadULong();

        for (int i = 0; i < Costume.MaxCostumeItems; i++)
        {
            var part = new CostumeItem();
            part.Read(reader);
            Items[i] = part;
        }

        VisibilityMask = reader.ReadUInt();
        UserServiceToken = reader.ReadBit();
    }
}
