namespace EventStoreBackup
{
    using System;
    using System.Collections.Generic;
    using Dapper;
    using Infrastructure;
    using Microsoft.Data.SqlClient;

    public class EventStoreReader
    {
        private FromTableHelper From { get; }
        private Func<SqlConnection> CreateConnection { get; }

        public EventStoreReader(EventStoreConfiguration configuration)
        {
            Validate.NotNull(() => configuration);
            Validate.NotEmpty(() => configuration.Connection);

            From = new FromTableHelper(configuration);
            CreateConnection = () => new SqlConnection(configuration.Connection);
        }

        public uint? GetCurrentPosition()
        {
            using (var connection = CreateConnection())
                return connection.ExecuteScalar<uint?>($"SELECT MAX(Position) {From.Streams}");
        }

        public IEnumerable<dynamic> ReadStreams()
        {
            using (var connection = CreateConnection())
                return connection.Query($"SELECT * {From.Streams}");
        }

        public IEnumerable<dynamic> ReadEvents(ulong? after, ulong to)
        {
            using (var connection = CreateConnection())
            {
                var query =
                    $"SELECT * {From.Messages} " +
                    $"WHERE Position <= {to} " +
                    (after.HasValue ? $"AND Position > {after.Value} " : "") +
                    "ORDER BY Position DESC";

                return connection.Query(query);
            }
        }

        private class FromTableHelper
        {
            public FromTableHelper(EventStoreConfiguration configuration)
            {
                Validate.NotNull(() => configuration);
                Validate.NotEmpty(
                    () => configuration.Schema,
                    () => configuration.MessagesTable,
                    () => configuration.StreamsTable
                );

                Streams = $"FROM {configuration.Schema}.{configuration.StreamsTable}";
                Messages = $"FROM {configuration.Schema}.{configuration.MessagesTable}";
            }

            public string Streams { get; }
            public string Messages { get; }
        }
    }
}
