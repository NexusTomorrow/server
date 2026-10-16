using System.Reflection;
using NexusTomorrow.Network.Codec.Tests.Fixtures;

namespace NexusTomorrow.Network.Codec.Tests.Verification;

public sealed class MessageVerificationTests
{
    private const string Key = "0xFFFF";
    private const string Fixtures = "NexusTomorrow.Network.Codec.Tests.Fixtures.";
    private static readonly Assembly[] s_fixtures = [typeof(FieldsFixture).Assembly];

    private static Dictionary<string, string> Summaries(params (string Id, string Text)[] entries) =>
        entries.ToDictionary(e => e.Id, e => e.Text, StringComparer.Ordinal);

    [Fact]
    public void AMessageWithNoRungNeedsNoDocumentation()
    {
        Assert.Empty(MessageVerification.Problems(typeof(UnreadFixture), Key, Summaries(), s_fixtures));
    }

    [Fact]
    public void PurposeNeedsASummaryOnTheType()
    {
        Assert.Equal(["0xFFFF: Purpose needs a summary on PurposeFixture"], MessageVerification.Problems(typeof(PurposeFixture), Key, Summaries(), s_fixtures));
        Assert.Empty(MessageVerification.Problems(typeof(PurposeFixture), Key, Summaries(($"T:{Fixtures}PurposeFixture", "What it is for.")), s_fixtures));
    }

    [Fact]
    public void FieldsNeedsASummaryOnEveryMemberOfEveryLayoutTheMessageReaches()
    {
        List<string> problems = MessageVerification.Problems(typeof(FieldsFixture), Key, Summaries(($"T:{Fixtures}FieldsFixture", "What it is for.")), s_fixtures);

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
    public void FieldsChecksWireMembersOfAnyAccessibility()
    {
        Dictionary<string, string> summaries = Summaries(
            ($"T:{Fixtures}PrivateMemberFixture", "What it is for."),
            ($"P:{Fixtures}PrivateMemberFixture.Shown", "A value."));

        Assert.Equal(
            ["0xFFFF: Fields needs a summary on PrivateMemberFixture.Hidden"],
            MessageVerification.Problems(typeof(PrivateMemberFixture), Key, summaries, s_fixtures));
    }

    [Fact]
    public void SummariesFromSeveralAssembliesMergeIntoOneLookup()
    {
        Dictionary<string, string> summaries = XmlDocs.SummariesOf(
            [typeof(global::NexusTomorrow.Network.Auth.WireMessages).Assembly, typeof(global::NexusTomorrow.Network.WireMessages).Assembly]);

        Assert.Equal("A GUID in the Windows layout, in wire order: a 32-bit part, two 16-bit parts and eight bytes.", summaries["T:NexusTomorrow.Network.NetworkGuid"]);
        Assert.Equal("The first 32 bits.", summaries["P:NexusTomorrow.Network.NetworkGuid.Data1"]);
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
            MessageVerification.Problems(typeof(FieldsFixture), Key, summaries, s_fixtures));

        summaries[$"P:{Fixtures}VerifiedArm.Unknown0"] = MessageVerification.IgnoredByTheClient + " It is always zero on the wire.";
        Assert.Empty(MessageVerification.Problems(typeof(FieldsFixture), Key, summaries, s_fixtures));
    }
}
