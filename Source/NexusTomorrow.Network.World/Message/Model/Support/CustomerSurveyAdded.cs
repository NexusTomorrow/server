using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Offers the player a customer survey.
/// </summary>
/// <remarks>
/// The client adds the survey and its object to its pending surveys unless that pair is already pending, keeps at most 50, and raises
/// NewCustomerSurveyRequest with the count. The player answers with <see cref="CustomerSurveyResponse"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Server033E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CustomerSurveyAdded
{
    /// <summary>
    /// The survey, as its CustomerSurvey row id: one of the CustomerSurveyLib.CodeEnumCustomerSurvey values.
    /// </summary>
    [UInt(14)] public ushort SurveyId { get; set; }

    /// <summary>
    /// What the survey is about; the client resolves it by the survey's type for the survey's title.
    /// </summary>
    [UInt(32)] public uint ObjectId { get; set; }
}
