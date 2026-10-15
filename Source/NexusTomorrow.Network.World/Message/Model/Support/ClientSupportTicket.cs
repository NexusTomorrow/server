using System.Numerics;
using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.ClientSupportTicket, Direction.ClientToServer)]
public sealed partial class ClientSupportTicket
{
    [UInt(16)] public ushort TicketCategoryId { get; private set; }
    [UInt(16)] public ushort TicketSubCategoryId { get; private set; }
    [Float32] public Vector3 Position { get; private set; } // player unit's current commanded position
    [WideString] public string Subject { get; private set; }
    [WideString] public string Body { get; private set; }
    [UInt(32)] public Language LanguageId { get; private set; }
}
