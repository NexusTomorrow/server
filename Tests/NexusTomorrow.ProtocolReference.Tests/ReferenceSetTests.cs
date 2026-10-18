using System.Text;

namespace NexusTomorrow.ProtocolReference.Tests;

public sealed class ReferenceSetTests : IDisposable
{
    private const string Sentence = "This page's messages moved to their feature areas; [Messages](Messages) lists every message and the page that describes it.";

    private readonly string _folder = Directory.CreateTempSubdirectory("reference-set-").FullName;

    public void Dispose() => Directory.Delete(_folder, recursive: true);

    private static string Tombstone(string shieldsDate, string date) =>
        "![index, committed](https://img.shields.io/badge/index-committed-2ea44f) "
        + $"![verified {date}](https://img.shields.io/badge/verified-{shieldsDate}-6e7781)\n\n{Sentence}\n";

    private string PathOf(string name) => Path.Combine(_folder, name);

    [Fact]
    public void WritingASetLeavesATombstoneForAMessagePageItNoLongerHasAndTouchesNothingElse()
    {
        File.WriteAllText(PathOf("Messages-Retired.md"), "an area that no longer exists\n");
        File.WriteAllText(PathOf("Messages-Pregame.md"), "the old text\n");
        File.WriteAllText(PathOf("Home.md"), "a page the tool does not own\n");

        IReadOnlyList<string> retired = ReferenceSet.Write(_folder, [("Messages.md", "index\n"), ("Messages-Pregame.md", "pregame\n")], "2026-10-17");

        Assert.Equal(["Messages-Retired.md"], retired);
        Assert.Equal("index\n", File.ReadAllText(PathOf("Messages.md")));
        Assert.Equal("pregame\n", File.ReadAllText(PathOf("Messages-Pregame.md")));
        Assert.Equal("a page the tool does not own\n", File.ReadAllText(PathOf("Home.md")));
        Assert.Equal(Tombstone("2026--10--17", "2026-10-17"), File.ReadAllText(PathOf("Messages-Retired.md")));
    }

    [Fact]
    public void AnAreaDroppedBetweenRunsBecomesATombstoneAndIsReportedOnce()
    {
        ReferenceSet.Write(_folder, [("Messages-A.md", "a\n"), ("Messages-B.md", "b\n")], "2026-10-16");

        IReadOnlyList<string> second = ReferenceSet.Write(_folder, [("Messages-A.md", "a\n")], "2026-10-17");

        Assert.Equal(["Messages-B.md"], second);
        Assert.Equal(Tombstone("2026--10--17", "2026-10-17"), File.ReadAllText(PathOf("Messages-B.md")));

        IReadOnlyList<string> third = ReferenceSet.Write(_folder, [("Messages-A.md", "a\n")], "2026-11-02");

        Assert.Empty(third);
        Assert.Equal(Tombstone("2026--10--17", "2026-10-17"), File.ReadAllText(PathOf("Messages-B.md")));
        Assert.Equal(Encoding.UTF8.GetBytes(Tombstone("2026--10--17", "2026-10-17")), File.ReadAllBytes(PathOf("Messages-B.md")));
    }

    [Fact]
    public void ATombstoneForAListedNameIsWrittenWhenTheFileIsMissing()
    {
        IReadOnlyList<string> retired = ReferenceSet.Write(_folder, [("Messages.md", "index\n")], "2026-10-17");

        Assert.Empty(retired);
        Assert.Equal(Tombstone("2026--10--17", "2026-10-17"), File.ReadAllText(PathOf("Messages-Unsorted.md")));
    }

    [Fact]
    public void AnExistingTombstoneForAListedNameIsLeftAlone()
    {
        string earlier = Tombstone("2026--09--01", "2026-09-01");
        File.WriteAllText(PathOf("Messages-Unsorted.md"), earlier);

        IReadOnlyList<string> retired = ReferenceSet.Write(_folder, [("Messages.md", "index\n")], "2026-10-17");

        Assert.Empty(retired);
        Assert.Equal(earlier, File.ReadAllText(PathOf("Messages-Unsorted.md")));
    }

    [Fact]
    public void AnExistingTombstoneWithCrLfLineEndsIsLeftAloneAndNotReported()
    {
        byte[] earlier = Encoding.UTF8.GetBytes(Tombstone("2026--09--01", "2026-09-01").Replace("\n", "\r\n", StringComparison.Ordinal));
        File.WriteAllBytes(PathOf("Messages-B.md"), earlier);

        IReadOnlyList<string> retired = ReferenceSet.Write(_folder, [("Messages-A.md", "a\n")], "2026-10-17");

        Assert.Empty(retired);
        Assert.Equal(earlier, File.ReadAllBytes(PathOf("Messages-B.md")));
    }

    [Fact]
    public void AListedNameThatReturnsAsAnAreaIsWrittenAsARealPage()
    {
        ReferenceSet.Write(_folder, [("Messages.md", "index\n")], "2026-10-16");

        ReferenceSet.Write(_folder, [("Messages.md", "index\n"), ("Messages-Unsorted.md", "real page\n")], "2026-10-17");

        Assert.Equal("real page\n", File.ReadAllText(PathOf("Messages-Unsorted.md")));
    }

    [Fact]
    public void PagesAreWrittenAsUtf8WithoutAByteOrderMark()
    {
        ReferenceSet.Write(_folder, [("Messages.md", "é\n")], "2026-10-17");

        Assert.Equal([0xC3, 0xA9, 0x0A], File.ReadAllBytes(PathOf("Messages.md")));
    }
}
