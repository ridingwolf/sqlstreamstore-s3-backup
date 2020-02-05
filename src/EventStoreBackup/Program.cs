namespace EventStoreBackup
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;
    using Infrastructure;

    class Program
    {

        static async Task Main(string[] args)
        {
            var service = new BackupService(Configure());
            await service.Backup();
        }

        private static BackupServiceConfiguration Configure()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment.MachineName.ToLowerInvariant()}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return new BackupServiceConfiguration(config);
        }
    }
}
