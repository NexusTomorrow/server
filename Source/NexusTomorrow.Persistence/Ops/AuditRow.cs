namespace NexusTomorrow.Persistence.Ops;

#pragma warning disable CS1591 // Data-shape code: the type's summary names the table; a column's name is its meaning (code standard, Documentation).

/// <summary>
/// <c>ops.audit</c>: one admin action, allowed or denied.
/// </summary>
public sealed class AuditRow
{
    public long Id { get; set; }

    public DateTimeOffset At { get; set; }

    public string Actor { get; set; } = "";

    public string FrontEnd { get; set; } = "";

    public string Action { get; set; } = "";

    public string? Target { get; set; }

    public string? Details { get; set; }

    public string? RemoteAddress { get; set; }

    public string Outcome { get; set; } = "";
}
