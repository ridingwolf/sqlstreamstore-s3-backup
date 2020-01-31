namespace EventStoreBackup.Infrastructure
{
    internal interface IBackupServiceConfiguration
    {
        int BatchSize { get; }
        string EventStoreConnectionString { get; }
    }
}