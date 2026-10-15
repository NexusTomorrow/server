using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity;

[WireStruct]
public sealed partial class Position : IReadable, IWritable
{
    [Float32] public Vector3 Vector { get; private set; }

    public Position()
    {
    }

    public Position(Vector3 vector)
    {
        Vector = vector;
    }

    public void Read(GamePacketReader reader)
    {
        Vector = new Vector3(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle());
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Vector.X);
        writer.Write(Vector.Y);
        writer.Write(Vector.Z);
    }
}
