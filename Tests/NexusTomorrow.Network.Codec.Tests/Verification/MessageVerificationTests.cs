using NexusTomorrow.Network.Codec.Tests.Fixtures;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

public sealed class MessageVerificationTests
{
    private const string Key = "0xFFFF";
    private const string Fixtures = "NexusTomorrow.Network.Codec.Tests.Fixtures.";

    private static Dictionary<string, string> Summaries(params (string Id, string Text)[] entries) =>
        entries.ToDictionary(e => e.Id, e => e.Text, StringComparer.Ordinal);

    [Fact]
    public void AMessageWithNoRungNeedsNoDocumentation()
    {
        Assert.Empty(MessageVerification.Problems(typeof(UnreadFixture), Key, Summaries()));
    }

    [Fact]
    public void PurposeNeedsASummaryOnTheType()
    {
        Assert.Equal(["0xFFFF: Purpose needs a summary on PurposeFixture"], MessageVerification.Problems(typeof(PurposeFixture), Key, Summaries()));
        Assert.Empty(MessageVerification.Problems(typeof(PurposeFixture), Key, Summaries(($"T:{Fixtures}PurposeFixture", "What it is for."))));
    }

    [Fact]
    public void FieldsNeedsASummaryOnEveryMemberOfEveryLayoutTheMessageReaches()
    {
        List<string> problems = MessageVerification.Problems(typeof(FieldsFixture), Key, Summaries(($"T:{Fixtures}FieldsFixture", "What it is for.")));

        Assert.Equal(
            [
                "0xFFFF: Fields needs a summary on FieldsFixture.Part",
                "0xFFFF: Fields needs a summary on FieldsFixture.Payload",
                "0xFFFF: Fields needs a summary on FieldsFixture.Selector",
                "0xFFFF: Fields needs a summary on VerifiedArm.Unknown0",
                "0xFFFF: Fields needs a summary on VerifiedPart.Speed",
            ],
            problems.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void ANeutralNameAtFieldsNeedsTheIgnoredSentence()
    {
        Dictionary<string, string> summaries = Summaries(
            ($"T:{Fixtures}FieldsFixture", "What it is for."),
            ($"P:{Fixtures}FieldsFixture.Selector", "Which payload follows."),
            ($"P:{Fixtures}FieldsFixture.Payload", "The payload."),
            ($"P:{Fixtures}FieldsFixture.Part", "The part."),
            ($"P:{Fixtures}VerifiedPart.Speed", "Metres per second."),
            ($"P:{Fixtures}VerifiedArm.Unknown0", "Something."));

        Assert.Equal(
            ["0xFFFF: VerifiedArm.Unknown0 keeps a neutral name, so its summary must begin with the ignored-by-the-client sentence"],
            MessageVerification.Problems(typeof(FieldsFixture), Key, summaries));

        summaries[$"P:{Fixtures}VerifiedArm.Unknown0"] = MessageVerification.IgnoredByTheClient + " It is always zero on the wire.";
        Assert.Empty(MessageVerification.Problems(typeof(FieldsFixture), Key, summaries));
    }
}
