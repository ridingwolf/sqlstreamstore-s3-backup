namespace EventStoreBackup.Infrastructure
{
    public class EventStoreConfiguration
    {
        public string Connection { get; set; }
        public string Schema { get; set; }
        public string MessagesTable { get; set; }
        public string StreamsTable { get; set; }
    }
}
