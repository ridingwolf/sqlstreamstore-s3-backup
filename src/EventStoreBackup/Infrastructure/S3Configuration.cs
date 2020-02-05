namespace EventStoreBackup.Infrastructure
{
    public class S3Configuration
    {
        public string Region { get; set; }
        public string Bucket { get; set; }
        public string EventStoreName { get; set; }
    }
}
