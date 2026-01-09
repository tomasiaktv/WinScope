using System;
using System.Threading;
using System.Windows;
using WinScope.App.ViewModels;
using WinScope.Platform.Providers;

namespace WinScope.App;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;
    private readonly CancellationTokenSource _cts = new();

    public MainWindow()
    {
        InitializeComponent();

        _vm = new MainViewModel(new DiagnosticsProcessSnapshotProvider());
        DataContext = _vm;

        Loaded += async (_, __) =>
        {
            try
            {
                await _vm.RunAutoRefreshAsync(TimeSpan.FromSeconds(2), _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // normal on close
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        };

        Closing += (_, __) =>
        {
            _cts.Cancel();
            _cts.Dispose();
        };
    }
}
