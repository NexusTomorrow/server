using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// The player's answers to a pending customer survey; the client drops the survey from its pending surveys as it sends them.
/// </summary>
/// <remarks>
/// The client writes this one layout whatever the survey.
/// </remarks>
[WireMessage(GameMessageOpcode.Client033F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CustomerSurveyResponse
{
    /// <summary>
    /// The survey, as <see cref="CustomerSurveyAdded.SurveyId"/> gave it.
    /// </summary>
    [UInt(14)] public ushort SurveyId { get; private set; }

    /// <summary>
    /// What the survey is about, as <see cref="CustomerSurveyAdded.ObjectId"/> gave it.
    /// </summary>
    [UInt(32)] public uint ObjectId { get; private set; }

    /// <summary>
    /// Up to four answers, one per question.
    /// </summary>
    [FixedArray(4), UInt(8)] public byte[] Responses { get; private set; } = new byte[4];

    /// <summary>
    /// The player's comment.
    /// </summary>
    [WideString] public string Comment { get; private set; } = "";
}
