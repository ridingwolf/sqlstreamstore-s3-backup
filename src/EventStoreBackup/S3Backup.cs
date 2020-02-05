namespace EventStoreBackup
{
    using System;
    using Infrastructure;

    public class S3Backup
    {
        private readonly S3Configuration _configuration;
        private readonly EventStoreReader _store;
        private readonly uint _batchSize;

        public S3Backup(S3Configuration configuration, uint batchSize, EventStoreReader store)
        {
            Validate.NotNull(
                () => configuration,
                () => store
            );
            Validate.NotEmpty(
                () => configuration.Bucket,
                () => configuration.Region
            );
            Validate.NotZero(() => batchSize);

            _configuration = configuration;
            _batchSize = batchSize;
            _store = store;
        }

        public ulong? GetLastBackupPosition()
        {

            // get last postion from S3 
            // pos = mostRecentFile.match("^\d{6}_\d+_(\d+)\.zip")

            return null;
        }
        
        //ToDo: move to correct class (is this a part of S3backup?)
        public void BackupFromPosition(ulong? lastBackupPosition)
        {
            // create s3 object : zip
            //      create zip to write into s3 (temp name)
            //          create streams csv
            //          stream streams data into csv
            //          parse stream positions while streaming -> result = current position to use a limit for messages

            //         while(current postion not reached)
            //              create messages csv FromPosition-ToPosition
            //              stream messages to csv

            //  rename s3 object to YYYYMMDD_posFROM-postUNTIL.zip

            var streams = _store.ReadStreams();
            

            // todo write the damn things


            // rename s3 object (copy, delete original)
            //      file format: YYYYMMDD_posFROM-postUNTIL.zip
            throw new NotImplementedException();
        }
    }
}
