using Microsoft.Extensions.Options;

namespace MyExpenses.Models
{
    public class AppConfig
    {
        public bool UseInMemoryDatabase { get; set; }
        public bool ClearDatabaseAndSeedData { get; set; }

        public string ConnectionString { get; set; }
        public string BuildVersion { get; set; }
    }

    public interface IAppConfig
    {
        bool UseInMemoryDatabase { get; }
        bool ClearDatabaseAndSeedData { get; }

        string ConnectionString { get; }
        string BuildVersion { get; }
    }

    public class AppConfigModel : IAppConfig
    {
        private readonly IOptions<AppConfig> _appConfigOptions;

        public AppConfigModel(IOptions<AppConfig> appConfigOptions)
        {
            _appConfigOptions = appConfigOptions;
        }

        public bool UseInMemoryDatabase => _appConfigOptions.Value.UseInMemoryDatabase;

        public bool ClearDatabaseAndSeedData => _appConfigOptions.Value.ClearDatabaseAndSeedData;

        public string ConnectionString => _appConfigOptions.Value.ConnectionString;

        public string BuildVersion => _appConfigOptions.Value.BuildVersion;
    }
}