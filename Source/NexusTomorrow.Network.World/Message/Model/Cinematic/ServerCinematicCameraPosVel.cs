using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.Server0214, Direction.ServerToClient)]
public sealed partial class Server0214
{
    [UInt(32)] public uint Unknown0 { get; set; }
    public Client0096Part0 Unknown1 { get; set; }
    [Float32] public Vector3 Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
}

