using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

/// <summary>
/// A chat link's payload, one arm for each value of <see cref="ChatLink.Type"/>.
/// </summary>
public interface IChatLinkPayload : IWireStruct
{
}
