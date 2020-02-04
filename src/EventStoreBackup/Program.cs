
namespace EventStoreBackup
{
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using System;
    using Infrastructure;

    class Program
    {

        static void Main(string[] args)
        {
            var configuration = Configure();
            Backup(configuration);
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

        private static void Backup(BackupServiceConfiguration configuration)
        {
            Validate.NotNull(() => configuration.BatchSize);
            var store = new EventStoreReader(configuration.EventStore);
            var backup = new S3Backup(configuration.S3);

            var currentPosition = store.GetCurrentPosition();
            if (!currentPosition.HasValue)
                return;

            var lastMessagePosition = currentPosition.Value;
            var lastBackupPosition = backup.GetLastBackupPosition();
            
            while ( !lastBackupPosition.HasValue || lastBackupPosition.Value < lastMessagePosition)
            {
                var batchEndPosition =
                    Math.Min((lastBackupPosition ?? 0) + configuration.BatchSize, lastMessagePosition);


                //var batchEndPosition = lastBackupPosition.HasValue
                //    ? Math.Min(lastBackupPosition.Value + configuration.BatchSize, lastMessagePosition)
                //    : Math.Min(configuration.BatchSize, lastMessagePosition);

                // ignore stream-table for now
                backup.Write(store.ReadEvents, lastBackupPosition, batchEndPosition);
                lastBackupPosition = batchEndPosition;
            }
        }
    }
}
