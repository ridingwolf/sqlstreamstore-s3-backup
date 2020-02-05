using EventStoreBackup.Infrastructure;

namespace EventStoreBackup
{
    public class BackupService
    {
        private readonly EventStoreReader _store;
        private readonly S3Backup _backup;

        public BackupService(BackupServiceConfiguration configuration)
        {
            Validate.NotNull(() => configuration);
            _store = new EventStoreReader(configuration.EventStore);
            _backup = new S3Backup(configuration.S3, configuration.BatchSize , _store);
        }

        public void Backup()
        {
            var currentPosition = _store.GetCurrentPosition();
            if (!currentPosition.HasValue)
                return;

            var lastBackupPosition = _backup.GetLastBackupPosition();
            if (lastBackupPosition.HasValue && lastBackupPosition.Value >= currentPosition.Value)
                return;

            _backup.BackupFromPosition(lastBackupPosition);
        }
    }
}
