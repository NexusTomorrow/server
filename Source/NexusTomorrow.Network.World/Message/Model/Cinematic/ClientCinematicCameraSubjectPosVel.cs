using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Client071D, Direction.ClientToServer)]
public sealed partial class Client071D
{
    public Client0096Part0 Unknown0 { get; private set; }
    [Float32] public Vector3 Unknown1 { get; private set; }
}
