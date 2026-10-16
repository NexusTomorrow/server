namespace NexusTomorrow.ProtocolReference.Tests;

public sealed class ReferenceSetTests : IDisposable
{
    private readonly string _folder = Directory.CreateTempSubdirectory("reference-set-").FullName;

    public void Dispose() => Directory.Delete(_folder, recursive: true);

    [Fact]
    public void WritingASetRemovesTheMessagePagesItNoLongerHasAndNothingElse()
    {
        File.WriteAllText(Path.Combine(_folder, "Messages-Retired.md"), "an area that no longer exists\n");
        File.WriteAllText(Path.Combine(_folder, "Messages-Pregame.md"), "the old text\n");
        File.WriteAllText(Path.Combine(_folder, "Home.md"), "a page the tool does not own\n");

        IReadOnlyList<string> removed = ReferenceSet.Write(_folder, [("Messages.md", "index\n"), ("Messages-Pregame.md", "pregame\n")]);

        Assert.Equal(["Messages-Retired.md"], removed);
        Assert.Equal("index\n", File.ReadAllText(Path.Combine(_folder, "Messages.md")));
        Assert.Equal("pregame\n", File.ReadAllText(Path.Combine(_folder, "Messages-Pregame.md")));
        Assert.True(File.Exists(Path.Combine(_folder, "Home.md")));
        Assert.False(File.Exists(Path.Combine(_folder, "Messages-Retired.md")));
    }

    [Fact]
    public void PagesAreWrittenAsUtf8WithoutAByteOrderMark()
    {
        ReferenceSet.Write(_folder, [("Messages.md", "é\n")]);

        Assert.Equal([0xC3, 0xA9, 0x0A], File.ReadAllBytes(Path.Combine(_folder, "Messages.md")));
    }
}
