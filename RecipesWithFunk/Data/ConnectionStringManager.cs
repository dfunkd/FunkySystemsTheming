using System.Configuration;

namespace RecipesWithFunk.Data;

public static class ConnectionStringManager
{
    public static string? GetConnectionString()
    {
        string connectionString = string.Empty;

        string machineName = Environment.MachineName;
        if (machineName is not null)
        {
            if (machineName == "LWX2302PF2QFZY4")
                connectionString = ConfigurationManager.ConnectionStrings["WorkInventoryDb"].ConnectionString;
            else if (machineName == "DESKTOP-VL36BD6")
                connectionString = ConfigurationManager.ConnectionStrings["HomeInventoryDb"].ConnectionString;
        }

        return connectionString;
    }
}
