using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingChangeInteriorWallpaper, Direction.ClientToServer)]
public sealed partial class ClientHousingChangeInteriorWallpaper
{
    [FixedArray(6), UInt(32)] public uint[] LayerUpdated { get; private set; } = new uint[6];
    [FixedArray(6)] public DecorInfo[] DecorUpdates { get; private set; } = new DecorInfo[6];
}
