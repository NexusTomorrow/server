using Microsoft.CodeAnalysis;

namespace NexusTomorrow.ProtocolReference;

/// <summary>
/// One of the codec generator's diagnostics as its page shows it: the id, the severity it is reported at by default, and the
/// descriptor's description as the page's "When" text.
/// </summary>
public sealed record CodecDiagnostic(string Id, DiagnosticSeverity Severity, string When);
