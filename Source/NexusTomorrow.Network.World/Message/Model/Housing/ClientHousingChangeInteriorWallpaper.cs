using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingChangeInteriorWallpaper)]
[WireMessage(GameMessageOpcode.ClientHousingChangeInteriorWallpaper, Direction.ClientToServer)]
public sealed partial class ClientHousingChangeInteriorWallpaper : IReadable
{
    [FixedArray(6), UInt(32)] public uint[] LayerUpdated { get; private set; } = new uint[6];
    [FixedArray(6)] public DecorInfo[] DecorUpdates { get; private set; } = new DecorInfo[6];

    public void Read(GamePacketReader reader)
    {
        for (int i = 0; i < LayerUpdated.Length; i++)
            LayerUpdated[i] = reader.ReadUInt();

        for (int i = 0; i < DecorUpdates.Length; i++)
            DecorUpdates[i].Read(reader);
    }
}
