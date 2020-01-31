namespace EventStoreBackup
{
    using System;
    using Infrastructure;

    class Program
    {

        static void Main(string[] args)
        {
            //todo: properly do config
            IBackupServiceConfiguration configuration = new BackupConfiguration();

            var settings = new BackupSettings
            {
                LastBackupPosition = GetLastBackupPosition(),
                CurrentPosition = GetCurrentPosition()
            };

            Backup(settings);
        }

        private static ulong? GetCurrentPosition()
        {
            // get current position from store

            throw new NotImplementedException();
        }

        private static ulong? GetLastBackupPosition()
        {

            // get last postion from S3 
            // pos = mostRecentFile.match("^\d{6}_\d+_(\d+)\.zip")

            return null;
        }

        private static void Backup(BackupSettings settings)
        {
            // ignore stream-table for now

            // for (number of batches)
            //      create zip writer
            //      open s3-filestream for zip
            //      file format: YYYYMMDD_posFROM-postUNTIL.zip
            //      query messages and write csv to zip stream

            throw new NotImplementedException();
        }

        private class BackupSettings
        {
            public ulong? LastBackupPosition { get; set; }
            public ulong? CurrentPosition { get; set; }

            public bool RequiresBackup => CurrentPosition.HasValue &&
                                          ( !LastBackupPosition.HasValue ||
                                            LastBackupPosition.Value < CurrentPosition.Value);
        }
    }
}
