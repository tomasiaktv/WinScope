using WinScope.Models.Processes;

namespace WinScope.Core.Abstractions;

public interface IProcessSnapshotProvider
{
    Task<IReadOnlyList<ProcessInfo>> GetSnapshotAsync(CancellationToken ct);
}
