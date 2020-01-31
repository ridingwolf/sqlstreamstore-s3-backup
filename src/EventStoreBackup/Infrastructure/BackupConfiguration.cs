namespace EventStoreBackup.Infrastructure
{
    public class BackupConfiguration : IBackupServiceConfiguration
    {
        public int BatchSize => 1000000;
        public string EventStoreConnectionString => "";
    }
}
