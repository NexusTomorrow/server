using System.Numerics;
using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingPlaceDecor)]
[WireMessage(GameMessageOpcode.ClientHousingPlaceDecor, Direction.ClientToServer)]
public sealed partial class ClientHousingPlaceDecor : IReadable
{
    public Identity ResidenceIdentity { get; private set; } = new();
    [UInt(64)] public ulong DecorId { get; private set; }
    [UInt(32)] public uint HousingDecorInfoId { get; private set; }
    [UInt(3)] public DecorOperation Operation { get; private set; }
    [Float32] public Vector3 Position { get; private set; }
    [Float32] public Quaternion Rotation { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ResidenceIdentity.Read(reader);
        DecorId = reader.ReadULong();
        HousingDecorInfoId = reader.ReadUInt();
        Operation = reader.ReadEnum<DecorOperation>(3u);
        Position = reader.ReadVector3();
        Rotation = new Quaternion(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle()
        );
    }
}
