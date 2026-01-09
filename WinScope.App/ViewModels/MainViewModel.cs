using System.Collections.ObjectModel;
using System.Linq;
using WinScope.Core.Abstractions;
using WinScope.Models.Processes;

namespace WinScope.App.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly IProcessSnapshotProvider _provider;

    private ProcessInfo? _selectedProcess;
    public ProcessInfo? SelectedProcess
    {
        get => _selectedProcess;
        set => SetField(ref _selectedProcess, value);
    }

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        private set => SetField(ref _isRefreshing, value);
    }

    public ObservableCollection<ProcessInfo> Processes { get; } = new();

    public MainViewModel(IProcessSnapshotProvider provider)
    {
        _provider = provider;
    }

    public async Task RefreshOnceAsync(CancellationToken ct)
    {
        if (IsRefreshing) return; // prevents overlap if something runs long
        IsRefreshing = true;

        try
        {
            var snapshot = await _provider.GetSnapshotAsync(ct);

            // Keep selection by PID if possible
            var selectedPid = SelectedProcess?.Pid;

            Processes.Clear();
            foreach (var item in snapshot.OrderBy(p => p.Name))
                Processes.Add(item);

            if (selectedPid is not null)
                SelectedProcess = Processes.FirstOrDefault(p => p.Pid == selectedPid);
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    public async Task RunAutoRefreshAsync(TimeSpan interval, CancellationToken ct)
    {
        // Initial load
        await RefreshOnceAsync(ct);

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(interval, ct);
            await RefreshOnceAsync(ct);
        }
    }
}
