using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server033D, Direction.ServerToClient)]
public sealed partial class Server033D
{
    [Flags]
    public enum RealmTransferFlags
    {
        FreeRealmTransfer = 0x1,
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
}
