using System.Data;
using System.Windows;
using Microsoft.Win32;
using PredictiveMarketing.App.Services;

namespace PredictiveMarketing.App;

public partial class MainWindow : Window
{
    private readonly DataService _dataService = new();
    private readonly PredictionService _predictionService = new();
    private DataTable? _loadedData;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void BtnLoadData_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select data file",
            Filter = "Data files (*.xlsx;*.csv)|*.xlsx;*.csv|Excel (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv|All files (*.*)|*.*",
            FilterIndex = 1
        };

        if (dialog.ShowDialog() != true)
            return;

        string filePath = dialog.FileName;
        TxtFileInfo.Text = System.IO.Path.GetFileName(filePath);
        TxtStatus.Text = "Loading...";
        BtnLoadData.IsEnabled = false;

        try
        {
            _loadedData = await _dataService.LoadDataAsync(filePath, takeRows: 500).ConfigureAwait(true);
            await Dispatcher.InvokeAsync(() =>
            {
                DataPreviewGrid.ItemsSource = _loadedData?.DefaultView;
                BtnTrain.IsEnabled = _loadedData != null && _loadedData.Rows.Count > 0;
                TxtStatus.Text = _loadedData != null ? "Data loaded" : "Ready";
            });
        }
        catch (System.Exception ex)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                DataPreviewGrid.ItemsSource = null;
                BtnTrain.IsEnabled = false;
                TxtStatus.Text = "Ready";
                MessageBox.Show(ex.Message, "Load Data", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
        }
        finally
        {
            BtnLoadData.IsEnabled = true;
        }
    }
}
