using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinScope.Models.Processes;

public sealed class ProcessInfo
{
    public required string Name { get; init; }
    public required int Pid { get; init; }

    public double CpuPercent { get; init; }     // later
    public long MemoryBytes { get; init; }      // milestone B

    public string? Path { get; init; }          // later
    public DateTime? StartTime { get; init; }   // later
    public int? ThreadCount { get; init; }      // later
    public int? HandleCount { get; init; }      // later
    public double MemoryMB => Math.Round(MemoryBytes / 1024d / 1024d, 1);
    public TimeSpan TotalProcessorTime { get; init; }

}