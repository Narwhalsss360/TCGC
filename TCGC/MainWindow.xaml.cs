using System.Windows;
using TruckConnect;

namespace TCGC;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    TimeSpan _refreshDelay = TimeSpan.FromMilliseconds(1000 / 24);
    System.Timers.Timer _refreshTimer = new(1000 / 24) { AutoReset = false };
    CancellationTokenSource _cts = new();
    GaugeClusterData _data = new();
    Connection _connection = new();
    DateTime _lastRefresh = DateTime.Now;

    public MainWindow()
    {
        InitializeComponent();

        _fuelGauge.Animator = (from, to) => new(from, to, DateTime.Now - _lastRefresh);
        _oilTemperatureGauge.Animator = _fuelGauge.Animator;
        _rpmGauge.Animator = _fuelGauge.Animator;
        _speedGauge.Animator = _fuelGauge.Animator;
        _voltageGauge.Animator = _fuelGauge.Animator;
        _oilPressureGauge.Animator = _fuelGauge.Animator;


        _refreshTimer.Elapsed += (sender, e) => { RefreshThread().Wait(); _refreshTimer.Start(); };
        Closed += (sender, e) =>
        {
            _cts.Cancel();
            _refreshTimer.Stop();
        };
        Loaded += (sender, e) =>
        {
            Setup().ContinueWith(task => _refreshTimer.Start());
        };
    }

    async Task Setup()
    {
        await _connection.ConnectAsync();
        await _connection.RegisterDataDefinitionAsync(_data);
    }

    async Task RefreshThread()
    {
        _data.TruckConfiguration = await _connection.RequestAsync<MasterStorage.ConfigurationStorage.ConfigurationTruckStorage>(TelemetryID.ConfigurationTruckInfo, null, _cts.Token);
        await Task.Run(async () => await _connection.RequestAsync(_data), _cts.Token);
        await Dispatcher.InvokeAsync(() => Apply(_data));
        _lastRefresh = DateTime.Now;
    }

    void Apply(GaugeClusterData data)
    {
        _fuelGauge.Value = data.FuelLevel.ValueOrDefault(0) * 100 / data.TruckConfiguration.FuelCapacity.ValueOrDefault(1);
        _oilTemperatureGauge.Value = Math.Max(100, data.OilTemperature.ValueOrDefault(0));
        _rpmGauge.Value = data.Rpm.ValueOrDefault(0);
        _odometerTextBlock.Text = $"{data.Odometer.ValueOrDefault(0):F0}";
        _speedGauge.Value = 3.6 * data.Speed.ValueOrDefault(0);
        _voltageGauge.Value = data.BatteryVoltage.ValueOrDefault(0);
        _oilPressureGauge.Value = data.OilPressure.ValueOrDefault(0);
    }
}
