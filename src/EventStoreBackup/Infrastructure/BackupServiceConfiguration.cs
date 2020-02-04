namespace EventStoreBackup.Infrastructure
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class BackupServiceConfiguration
    {
        public BackupServiceConfiguration(IConfiguration configuration)
        {
            BatchSize = configuration.GetValue<uint>("BatchSize");
            if (BatchSize == 0)
                throw new ArgumentException($"{nameof(BatchSize)} must be greater than 0");

            EventStore = configuration.GetSection("Events").Get<EventStoreConfiguration>();
            S3 = configuration.GetSection("S3").Get<S3Configuration>();
        }

        public uint BatchSize { get; }
        public EventStoreConfiguration EventStore { get; }
        public S3Configuration S3 { get; }
    }
}
