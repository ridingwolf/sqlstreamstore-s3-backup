namespace EventStoreBackup
{
    using System;
    using System.Collections.Generic;
    using Dapper;
    using Infrastructure;
    using Microsoft.Data.SqlClient;

    public class EventStoreReader
    {
        private readonly EventStoreConfiguration _configuration;

        public EventStoreReader(EventStoreConfiguration configuration)
        {
            Validate.NotNull(() => configuration);
            Validate.NotEmpty(
                () => configuration.Connection,
                () => configuration.Schema,
                () => configuration.MessagesTable,
                () => configuration.StreamsTable
            );
            _configuration = configuration;
        }
        
        private SqlConnection CreateConnection() => new SqlConnection(_configuration.Connection);

        public uint? GetCurrentPosition() => throw new NotImplementedException();

        public IEnumerable<dynamic> ReadEvents(ulong? from, ulong until)
        {
            using (var connection = CreateConnection())
            {
                var query = $"SELECT * FROM [table] " +
                            $"WHERE Position <= {until} " +
                            (from.HasValue ? $"AND Position > {from.Value} " : "") +
                            "ORDER BY Position DESC";

                return connection.Query(query);
            }
        }
    }

}
