namespace RecipesWithFunk.ViewModels;

public class DatabaseSettingsViewModel : BaseViewModel
{
    private bool integratedSecurity = true;
    public bool IntegratedSecurity
    {
        get => integratedSecurity;
        set
        {
            if (integratedSecurity != value)
            {
                integratedSecurity = value;
                OnPropertyChanged();
            }
        }
    }

    private bool trustedServerCertificate = true;
    public bool TrustedServerCertificate
    {
        get => trustedServerCertificate;
        set
        {
            if (trustedServerCertificate != value)
            {
                trustedServerCertificate = value;
                OnPropertyChanged();
            }
        }
    }

    private bool validConnection = false;
    public bool ValidConnection
    {
        get => validConnection;
        set
        {
            if (validConnection != value)
            {
                validConnection = value;
                OnPropertyChanged();
            }
        }
    }

    private int connectionLifetime = 15;
    public int ConnectionLifetime
    {
        get => connectionLifetime;
        set
        {
            if (connectionLifetime != value)
            {
                connectionLifetime = value;
                OnPropertyChanged();
            }
        }
    }

    private int connectionTimeout = 180;
    public int ConnectionTimeout
    {
        get => connectionTimeout;
        set
        {
            if (connectionTimeout != value)
            {
                connectionTimeout = value;
                OnPropertyChanged();
            }
        }
    }

    private int maxPoolSize = 200;
    public int MaxPoolSize
    {
        get => maxPoolSize;
        set
        {
            if (maxPoolSize != value)
            {
                maxPoolSize = value;
                OnPropertyChanged();
            }
        }
    }

    private int minPoolSize = 1;
    public int MinPoolSize
    {
        get => minPoolSize;
        set
        {
            if (minPoolSize != value)
            {
                minPoolSize = value;
                OnPropertyChanged();
            }
        }
    }

    private string dataSource;
    public string ServerName
    {
        get => dataSource;
        set
        {
            if (dataSource != value)
            {
                dataSource = value;
                OnPropertyChanged();
            }
        }
    }

    private string initialCatalog;
    public string InitialCatalog
    {
        get => initialCatalog;
        set
        {
            if (initialCatalog != value)
            {
                initialCatalog = value;
                OnPropertyChanged();
            }
        }
    }

    private string testResult;
    public string TestResult
    {
        get => testResult;
        set
        {
            if (testResult != value)
            {
                testResult = value;
                OnPropertyChanged();
            }
        }
    }

    private string title = "Database Settings";
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }
}
