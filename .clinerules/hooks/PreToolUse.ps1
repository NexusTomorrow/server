# Cline workspace PreToolUse hook, one shot per machine: brings C:\src\dotfiles up to date and applies it, so the agent's
# rules, hooks and mailbox take effect. The work runs detached (the hook must answer within Cline's time limit): the
# dotfiles' own sync script (commit, pull --rebase, push), then chezmoi apply without scripts. A marker in
# %LOCALAPPDATA%\ClineMailbox records that it ran, with the commands' output in the .log beside it; every later call
# returns at once. It never blocks a tool call. Windows PowerShell 5.1.
$ErrorActionPreference = 'SilentlyContinue'
[Console]::OutputEncoding = New-Object System.Text.UTF8Encoding $false
$null = [Console]::In.ReadToEnd()
$marker = Join-Path $env:LOCALAPPDATA 'ClineMailbox\dotfiles-sync-once.txt'
if (Test-Path -LiteralPath $marker) {
    [Console]::Out.Write('{"cancel":false}')
    exit 0
}
[void][IO.Directory]::CreateDirectory((Split-Path $marker))
Set-Content -LiteralPath $marker -Value (Get-Date -Format o)
$log = "$marker.log"
$sync = Join-Path $env:USERPROFILE '.claude\hooks\dotfiles-autocommit.ps1'
$command = "& '$sync' *>> '$log'; git -C C:/src/dotfiles status -sb *>> '$log'; chezmoi apply --force --exclude=scripts *>> '$log'; 'done' *>> '$log'"
Start-Process powershell -WindowStyle Hidden -ArgumentList @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-Command', $command)
$context = 'The dotfiles are syncing in the background (pull, then chezmoi apply); your rules, hooks and mailbox change when it finishes. Read every message for you in C:\src\dotfiles\Claude\projects\NexusTomorrow\mailbox\, oldest first, before your next step.'
[Console]::Out.Write((@{ cancel = $false; contextModification = $context } | ConvertTo-Json -Compress))
