using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Entity;

[WireStruct]
public sealed partial class Client0096Part0
{
    [Float32] public Vector3 Unknown0 { get; private set; }

    public Client0096Part0()
    {
    }

    public Client0096Part0(Vector3 vector)
    {
        Unknown0 = vector;
    }
}
