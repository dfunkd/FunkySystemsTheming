using Microsoft.Data.SqlClient;
using RecipesWithFunk.Properties;
using RecipesWithFunk.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecipesWithFunk.Windows;

public partial class DatabaseSettings : Window
{
    #region Routed Commands
    #region CancelCommand
    private static readonly RoutedCommand cancelCommand = new();
    public static RoutedCommand CancelCommand = cancelCommand;
    private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
        => Environment.Exit(0);
    #endregion

    #region SaveCommand
    private static readonly RoutedCommand saveCommand = new();
    public static RoutedCommand SaveCommand = saveCommand;
    private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = (DataContext is DatabaseSettingsViewModel vm) && vm.ValidConnection == true;
    private async void ExecutedSaveCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is DatabaseSettingsViewModel vm)
        {
            Settings.Default.ServerName = vm.ServerName;
            Settings.Default.InitialCatalog = vm.InitialCatalog;
            Settings.Default.Save();
            DialogResult = true;
            Close();
        }
    }
    #endregion

    #region TestConnectionCommand
    private static readonly RoutedCommand testConnectionCommand = new();
    public static RoutedCommand TestConnectionCommand = testConnectionCommand;
    private void CanExecuteTestConnectionCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = (DataContext is DatabaseSettingsViewModel vm) && !string.IsNullOrEmpty(vm.ServerName) && !string.IsNullOrEmpty(vm.InitialCatalog);
    private async void ExecutedTestConnectionCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is DatabaseSettingsViewModel vm)
        {
            vm.ValidConnection = IsServerConnected($"Data Source={vm.ServerName};Initial Catalog={vm.InitialCatalog};Integrated Security={vm.IntegratedSecurity};" +
                $"Connection Lifetime={vm.ConnectionLifetime};Min Pool Size={vm.MinPoolSize};Max Pool Size={vm.MaxPoolSize};Connection Timeout={vm.ConnectionTimeout};" +
                $"TrustServerCertificate={vm.TrustedServerCertificate};");
            vm.TestResult = vm.ValidConnection == true ? "Connected" : "Not Connected";
        }
    }
    #endregion
    #endregion

    public DatabaseSettings()
    {
        InitializeComponent();
    }

    #region Events
    private void OnClosing(object sender, CancelEventArgs e)
        => Application.Current.Shutdown();

    private void OnDrag(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }
    #endregion

    #region Functions
    private static bool IsServerConnected(string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
        finally
        {
            // not really necessary
            connection.Close();
        }
    }
    #endregion
}
