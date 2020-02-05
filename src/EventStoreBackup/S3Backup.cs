namespace EventStoreBackup
{
    using System;
    using Infrastructure;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Amazon.S3;
    using Amazon.S3.Model;

    public class S3Backup
    {
        private readonly EventStoreReader _store;
        private readonly uint _batchSize;
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucket;
        private readonly string _eventStoreName;


        public S3Backup(S3Configuration configuration, uint batchSize, EventStoreReader store)
        {
            Validate.NotNull(
                () => configuration,
                () => store
            );
            Validate.NotEmpty(
                () => configuration.Bucket,
                () => configuration.Region,
                () => configuration.EventStoreName
            );
            Validate.NotZero(() => batchSize);

            _batchSize = batchSize;
            _bucket = configuration.Bucket;
            _eventStoreName = configuration.EventStoreName;
            _store = store;
            _s3Client = new AmazonS3Client();
        }

        private readonly Regex _lastBackupPosition = new Regex(@"^\d{6}_\d+-(\d+)\.zip", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private ulong LastPositionFromBackup(Match backupFileMatch) =>
            ulong.TryParse(backupFileMatch.Groups[1].Value, out var position)
                ? position
                : throw new InvalidCastException($"Cannot select position from backup file '{backupFileMatch.Value}'");

        public async Task<ulong?> GetLastBackupPosition()
        {
            var request = new ListObjectsRequest
            {
                BucketName = _bucket,
                Prefix = _eventStoreName
            };

            var backupMatches = (await _s3Client.ListObjectsAsync(request))
                .S3Objects
                .Select(file => _lastBackupPosition.Match(file.Key))
                .Where(fileMatch => fileMatch.Success)
                .ToList();

            return backupMatches.Any()
                ? backupMatches.Max(LastPositionFromBackup)
                : (ulong?) null;
        }

        //ToDo: move to correct class (is this a part of S3backup?)
        public void BackupFromPosition(ulong? lastBackupPosition)
        {
            try
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
            // catch (AmazonS3Exception s3Exception)
            catch (Exception exception)
            {
                // on error clean up the (partially) uploaded file
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
