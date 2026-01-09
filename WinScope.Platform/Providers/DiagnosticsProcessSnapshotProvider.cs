using System.Diagnostics;
using WinScope.Core.Abstractions;
using WinScope.Models.Processes;

namespace WinScope.Platform.Providers;

public sealed class DiagnosticsProcessSnapshotProvider : IProcessSnapshotProvider
{
    private readonly Dictionary<int, TimeSpan> _previousCpuTimes = new();
    private DateTime _lastSampleTime = DateTime.UtcNow;
    private readonly int _processorCount = Environment.ProcessorCount;

    public Task<IReadOnlyList<ProcessInfo>> GetSnapshotAsync(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var elapsedMs = (now - _lastSampleTime).TotalMilliseconds;
        _lastSampleTime = now;

        var processes = Process.GetProcesses();
        var list = new List<ProcessInfo>(processes.Length);

        foreach (var p in processes)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                var totalCpu = p.TotalProcessorTime;

                double cpuPercent = 0;

                if (_previousCpuTimes.TryGetValue(p.Id, out var prevCpu) && elapsedMs > 0)
                {
                    var cpuUsedMs = (totalCpu - prevCpu).TotalMilliseconds;
                    cpuPercent = cpuUsedMs / (elapsedMs * _processorCount) * 100;
                }

                _previousCpuTimes[p.Id] = totalCpu;

                list.Add(new ProcessInfo
                {
                    Name = p.ProcessName,
                    Pid = p.Id,
                    MemoryBytes = p.WorkingSet64,
                    TotalProcessorTime = totalCpu,
                    CpuPercent = Math.Round(cpuPercent, 1)
                });
            }
            catch
            {
                // skip inaccessible processes
            }
            finally
            {
                p.Dispose();
            }
        }

        return Task.FromResult((IReadOnlyList<ProcessInfo>)list);
    }

}
