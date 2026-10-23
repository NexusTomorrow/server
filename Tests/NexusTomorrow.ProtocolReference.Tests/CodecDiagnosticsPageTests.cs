using Microsoft.CodeAnalysis;

namespace NexusTomorrow.ProtocolReference.Tests;

public sealed class CodecDiagnosticsPageTests
{
    private static readonly PageStamp s_stamp = new("dev", "abc1234", "2026-10-23");

    private static readonly CodecDiagnostic s_error = new("NTX001", DiagnosticSeverity.Error, "A member breaks the rule, such as `Count` set twice");

    private static readonly CodecDiagnostic s_warning = new("NTX002", DiagnosticSeverity.Warning, "A member bends the rule; it can be suppressed with its reason");

    [Fact]
    public void TheCodecDiagnosticsPageMatchesTheHandWrittenPage()
    {
        const string expected = """
            ![reference, committed](https://img.shields.io/badge/reference-committed-2ea44f) ![verified 2026-10-23](https://img.shields.io/badge/verified-2026--10--23-6e7781) [![dev abc1234](https://img.shields.io/badge/dev-abc1234-6e7781)](https://github.com/NexusTomorrow/server/commit/abc1234)

            Every diagnostic the codec's source generator reports: when it fires and how severe it is. How a declaration is written is on [Codec declarations](Codec-declarations).

            > [!NOTE]
            > Generated from the source generator's diagnostics at dev `abc1234` by `Tools/NexusTomorrow.ProtocolReference`. Don't edit it by hand: change the diagnostic's description and regenerate.

            The generator refuses to emit code for a type that breaks a rule, and the build fails. A type with any error gets no generated code, and neither does a type that nests or derives from it.

            | Id | Severity | When |
            |---|---|---|
            | **`NTX001`** | error | A member breaks the rule, such as `Count` set twice |
            | **`NTX002`** | warning, an error in our build | A member bends the rule; it can be suppressed with its reason |

            Each diagnostic sits on its declaration's syntax tree, so `.editorconfig` severities and `#pragma` reach it exactly as they reach the compiler's own.
            """;

        string page = PageWriter.CodecDiagnosticsPage([s_error, s_warning], s_stamp);

        Assert.Equal(expected.ReplaceLineEndings("\n") + "\n", page);
    }

    [Fact]
    public void TheRowsComeOutInIdOrderWhateverOrderTheDescriptorsAreFoundIn()
    {
        string[] blocks = PageWriter.CodecDiagnosticsPage([s_warning, s_error], s_stamp).Split("\n\n");

        Assert.StartsWith("| Id | Severity | When |\n|---|---|---|\n| **`NTX001`** | error |", blocks[4], StringComparison.Ordinal);
        Assert.Contains("\n| **`NTX002`** | warning, an error in our build |", blocks[4], StringComparison.Ordinal);
    }

    [Fact]
    public void EveryCodecDiagnosticHasADescriptionWithNoClosingFullStop()
    {
        IReadOnlyList<CodecDiagnostic> diagnostics = Registries.CodecDiagnostics();

        Assert.NotEmpty(diagnostics);
        foreach (CodecDiagnostic diagnostic in diagnostics)
        {
            Assert.False(string.IsNullOrWhiteSpace(diagnostic.When), $"{diagnostic.Id} has no description");
            Assert.False(diagnostic.When.EndsWith('.'), $"{diagnostic.Id}'s description ends with a full stop");
        }
    }

    [Fact]
    public void TheCodecDiagnosticsAreListedInIdOrderWithEachIdOnce()
    {
        string[] ids = [.. Registries.CodecDiagnostics().Select(diagnostic => diagnostic.Id)];

        Assert.NotEmpty(ids);
        Assert.Equal([.. ids.Order(StringComparer.Ordinal).Distinct()], ids);
    }
}
