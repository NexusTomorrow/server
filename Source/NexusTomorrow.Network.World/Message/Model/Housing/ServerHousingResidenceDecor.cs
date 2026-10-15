using System.Numerics;
using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

// This message is not for roofs, entryways, or doors. Client's handler for this message
// ignores decor items of these types.
[Message(GameMessageOpcode.ServerHousingResidenceDecor)]
[WireMessage(GameMessageOpcode.ServerHousingResidenceDecor, Direction.ServerToClient)]
public sealed partial class ServerHousingResidenceDecor : IWritable
{
    [WireStruct]
    public sealed partial class Decor : IWritable
    {
        public Identity ResidenceIdentity { get; set; }
        [UInt(64)] public ulong DecorId { get; set; }
        [UInt(32)] public DecorType DecorType { get; set; }
        [UInt(32)] public uint MannequinDecorId { get; set; } // Separate guid for each piece of mannequin items
        [UInt(32)] public uint Flags { get; set; } // Used as an override for housingWallpaperInfo.flags
        [UInt(32)] public uint HookIndex { get; set; }
        [UInt(32)] public uint PlotIndex { get; set; } = int.MaxValue;
        [Float32] public float Scale { get; set; }
        [Float32] public Vector3 Position { get; set; } = new();
        [Float32] public Quaternion Rotation { get; set; } = new();
        [UInt(32)] public uint HousingDecorInfoId { get; set; }
        [UInt(32)] public uint ActivePropUnitId { get; set; } // Prop unit the decor is attached to
        [UInt(64)] public ulong ParentDecorId { get; set; }
        [UInt(14)] public ushort ColourShift { get; set; }

        public void Write(GamePacketWriter writer)
        {
            ResidenceIdentity.Write(writer);
            writer.Write(DecorId);
            writer.Write(DecorType, 32u);
            writer.Write(MannequinDecorId);
            writer.Write(Flags);
            writer.Write(HookIndex);
            writer.Write(PlotIndex);
            writer.Write(Scale);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write(Rotation.X);
            writer.Write(Rotation.Y);
            writer.Write(Rotation.Z);
            writer.Write(Rotation.W);
            writer.Write(HousingDecorInfoId);
            writer.Write(ActivePropUnitId);
            writer.Write(ParentDecorId);
            writer.Write(ColourShift, 14u);
        }
    }
    [UInt(32), Count] public uint DecorDataCount { get; set; }

    [UInt(32)] public uint MessagesRemaining { get; set; } // decor data must be broken into chunks of 100 decor items per message, until this reaches 0 the client will not try to display the zone
    [Array(CountFrom = nameof(DecorDataCount))] public List<Decor> DecorData { get; set; } = []; // maximum 100 per message

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DecorData.Count);
        writer.Write(MessagesRemaining);
        DecorData.ForEach(decor => decor.Write(writer));
    }
}
