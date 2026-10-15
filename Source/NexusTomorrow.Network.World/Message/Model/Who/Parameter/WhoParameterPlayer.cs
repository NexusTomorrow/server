using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Who.Parameter;

[WireStruct]
public sealed partial class WhoParameterPlayer : IWhoParameterData
{
    [WideString] public string PlayerName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        PlayerName = reader.ReadWideString();
    }
}
