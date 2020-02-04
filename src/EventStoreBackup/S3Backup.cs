namespace EventStoreBackup
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;

    public class S3Backup
    {
        private readonly S3Configuration _configuration;

        public S3Backup(S3Configuration configuration)
        {
            Validate.NotNull(() => configuration);
            Validate.NotEmpty(
                () => configuration.Bucket,
                () => configuration.Region
            );
            _configuration = configuration;
        }

        public ulong? GetLastBackupPosition()
        {

            // get last postion from S3 
            // pos = mostRecentFile.match("^\d{6}_\d+_(\d+)\.zip")

            return null;
        }


        public void Write(Func<ulong?, ulong, IEnumerable<dynamic>> readEvents, ulong? lastBackupPosition, in ulong batchEndPosition)
        {
            //      create zip writer
            //      open s3-filestream for zip
            //      file format: YYYYMMDD_posFROM-postUNTIL.zip

            //      query messages and write csv to zip stream
            var events = readEvents(lastBackupPosition, batchEndPosition);

            // todo write the damn things

            throw new NotImplementedException();
        }
    }
}
