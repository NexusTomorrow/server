using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity;

[WireStruct]
public sealed partial class Position
{
    [Float32] public Vector3 Vector { get; private set; }

    public Position()
    {
    }

    public Position(Vector3 vector)
    {
        Vector = vector;
    }
}
